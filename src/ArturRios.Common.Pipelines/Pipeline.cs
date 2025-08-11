using System.Collections.Concurrent;
using ArturRios.Common.Pipelines.Commands;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Commands.IO;
using ArturRios.Common.Pipelines.Events;
using ArturRios.Common.Pipelines.Events.Interfaces;
using ArturRios.Common.Pipelines.Interfaces;
using ArturRios.Common.Pipelines.Queries;
using ArturRios.Common.Pipelines.Waiter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ArturRios.Common.Pipelines;

public class Pipeline : IPipeline
{
    private const int DefaultMaxRetryCount = 3;
    private const string UniqueConstraintViolationSqlState = "23505";
    private readonly IServiceProvider _serviceProvider;
    private readonly IPipelineConfiguration _configuration;
    private static readonly ConcurrentDictionary<Type, CommandHandlerWrapper> s_commandHandlerWrappers = new();
    private static readonly ConcurrentDictionary<Type, EventHandlerWrapper> s_eventHandlerWrappers = new();
    private static readonly ConcurrentDictionary<Type, QueryHandlerWrapper> s_queryHandlerWrappers = new();

    public Pipeline(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _configuration = _serviceProvider.GetService<IPipelineConfiguration>() ?? new PipelineConfiguration
        {
            MaxRetryCount = DefaultMaxRetryCount
        };
    }

    public async Task<PipelineOutput> ExecuteCommandAsync(object command)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<Pipeline>>();
        var backoffWaiter = new JitteredWaiter(Math.Max(_configuration.MaxRetryCount, 0));

        while (true)
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IPipelineContextFactory>();

            await using var dbContext = dbContextFactory.GetContext();
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var domainEventBus = scope.ServiceProvider.GetRequiredService<IDomainEventBus>();

            var commandHandler = s_commandHandlerWrappers.GetOrAdd(command.GetType(), static commandType =>
            {
                var commandInterface = commandType.GetInterfaces().First(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>));

                var returnType = commandInterface.GetGenericArguments()[0];

                var wrapperType = typeof(CommandHandlerWrapper<,>).MakeGenericType(commandType, returnType);

                var wrapperInstance = Activator.CreateInstance(wrapperType) ??
                                      throw new InvalidOperationException(
                                          $"Unable to create {nameof(CommandHandlerWrapper<ICommand<CommandOutput>, CommandOutput>)}<{commandType.Name}, {returnType.Name}> instance");

                return (CommandHandlerWrapper)wrapperInstance;
            });

            // TODO: return result on PipelineOutput
            object? result;

            try
            {
                logger.LogDebug("Executing command: {CommandType}", command.GetType().Name);

                result = await commandHandler.ExecuteAsync(command, scope.ServiceProvider);

                logger.LogDebug("Executed command: {CommandType}", command.GetType().Name);
                logger.LogDebug("Committing database transaction for command: {CommandType}", command.GetType().Name);

                await domainEventBus.Dispatch(dbContext);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex) when (IsRetryableException(ex))
            {
                if (backoffWaiter.CanRetry)
                {
                    logger.LogWarning(ex,
                        "Retryable exception occurred while executing command: {CommandType}. Retrying...",
                        command.GetType().Name);
                    await transaction.RollbackAsync();
                    await backoffWaiter.Wait();
                    continue;
                }

                logger.LogError(ex,
                    "A retryable exception was thrown but the maximum retry count of {MaxRetryCount} was reached for the command {CommandType}",
                    backoffWaiter.MaxRetryCount, command.GetType().Name);

                return new PipelineOutput
                {
                    Success = false,
                    Messages = ["A retryable exception was thrown but the maximum retry count was reached."]
                };
            }
            catch (Exception ex)
            {
                return new PipelineOutput { Success = false, Messages = [ex.Message] };
            }

            foreach (var domainEvent in domainEventBus.DomainEvents)
            {
                var eventHandler = s_eventHandlerWrappers.GetOrAdd(domainEvent.GetType(), static eventType =>
                {
                    var wrapperType = typeof(EventHandlerWrapper<>).MakeGenericType(eventType);
                    var wrapperInstance = Activator.CreateInstance(wrapperType) ??
                                          throw new InvalidOperationException(
                                              $"Unable to create {nameof(EventHandlerWrapper<object>)} instance for {eventType.Name}");

                    return (EventHandlerWrapper)wrapperInstance;
                });

                logger.LogDebug("Processing event: {EventType}: {DomainEventData}", domainEvent.GetType().FullName,
                    domainEvent);

                await eventHandler.Handle(domainEvent, scope.ServiceProvider);
            }

            var commandQueue = scope.ServiceProvider.GetRequiredService<ICommandQueue>();

            await commandQueue.Flush();

            return new PipelineOutput { Success = true, Messages = ["Command executed successfully."] };
        }
    }

    public async Task<PipelineOutput> ExecuteCommandAsync<TCommand, TOutput>(TCommand command)
        where TCommand : ICommand<TOutput>
        where TOutput : CommandOutput
    {
        return await ExecuteCommandAsync(command);
    }

    // TODO
    public Task<PipelineOutput<T>> ExecuteCommandAsync<TCommand, TInput, TOutput, T>(TCommand command)
        where TCommand : ICommand<TInput, TOutput, T>
        where TInput : CommandInput
        where TOutput : CommandOutput<T> =>
            throw new NotImplementedException();

    public async Task<PipelineOutput> ExecuteQueryAsync(object query)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<Pipeline>>();

        using var scope = _serviceProvider.CreateScope();

        var queryHandler = s_queryHandlerWrappers.GetOrAdd(query.GetType(), static queryType =>
        {
            var queryInterface = queryType.GetInterfaces().First(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));

            var returnType = queryInterface.GetGenericArguments()[0];

            var wrapperType = typeof(QueryHandlerWrapper<,>).MakeGenericType(queryType, returnType);

            var wrapperInstance = Activator.CreateInstance(wrapperType) ??
                                  throw new InvalidOperationException(
                                      $"Unable to create {nameof(QueryHandlerWrapper<IQuery<object>, object>)}<{queryType.Name}, {returnType.Name}> instance");

            return (QueryHandlerWrapper)wrapperInstance;
        });

        // TODO: return result on PipelineOutput
        object? result;

        logger.LogDebug("Executing query: {QueryType}", query.GetType().Name);

        result = await queryHandler.ExecuteAsync(query, scope.ServiceProvider);

        return new PipelineOutput
        {
            Success = true,
            Messages = ["Query executed successfully"]
        };
    }

    public async Task<TResult> ExecuteQueryAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult> where TResult : class
    {
        var result = await ExecuteQueryAsync(query);

        if (!result.Success)
        {
            throw new InvalidOperationException($"Query execution failed: {string.Join(", ", result.Messages)}");
        }

        // TODO: return result data
        throw new NotImplementedException();
    }

    private static bool IsRetryableException(Exception ex)
    {
        return ex switch
        {
            DbUpdateConcurrencyException
                or DbUpdateException
                {
                    InnerException: PostgresException { SqlState: UniqueConstraintViolationSqlState }
                } => true,
            _ => false
        };
    }
}
