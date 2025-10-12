using System.Collections.Concurrent;
using System.Reflection;
using ArturRios.Common.Pipelines.Commands;
using ArturRios.Common.Pipelines.Commands.Interfaces;
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

    public PipelineOutput ExecuteCommand(object command)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<Pipeline>>();
        var backoffWaiter = new JitteredWaiter(Math.Max(_configuration.MaxRetryCount, 0));

        while (true)
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IPipelineContextFactory>();

            var dbContext = dbContextFactory.GetContext();
            var transaction = dbContext.Database.BeginTransaction();

            var domainEventBus = scope.ServiceProvider.GetRequiredService<IDomainEventBus>();

            var commandHandler = s_commandHandlerWrappers.GetOrAdd(command.GetType(), static commandType =>
            {
                var returnType = ResolveCommandOutputType(commandType);

                var wrapperType = typeof(CommandHandlerWrapper<,>).MakeGenericType(commandType, returnType);

                var wrapperInstance = Activator.CreateInstance(wrapperType) ??
                                      throw new InvalidOperationException(
                                          $"Unable to create {nameof(CommandHandlerWrapper<ICommand, CommandOutput>)}<{commandType.Name}, {returnType.Name}> instance");

                return (CommandHandlerWrapper)wrapperInstance;
            });

            object? result;

            try
            {
                logger.LogDebug("Executing command: {CommandType}", command.GetType().Name);

                result = commandHandler.Handle(command, scope.ServiceProvider);

                logger.LogDebug("Executed command: {CommandType}", command.GetType().Name);
                logger.LogDebug("Committing database transaction for command: {CommandType}", command.GetType().Name);

                domainEventBus.Dispatch(dbContext);
                dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex) when (IsRetryableException(ex))
            {
                if (backoffWaiter.CanRetry)
                {
                    logger.LogWarning(ex,
                        "Retryable exception occurred while executing command: {CommandType}. Retrying...",
                        command.GetType().Name);
                    transaction.Rollback();
                    _ = backoffWaiter.Wait();

                    continue;
                }

                logger.LogError(ex,
                    "A retryable exception was thrown but the maximum retry count of {MaxRetryCount} was reached for the command {CommandType}",
                    backoffWaiter.MaxRetryCount, command.GetType().Name);

                return PipelineOutput.New.WithError(
                    "A retryable exception was thrown but the maximum retry count was reached");
            }
            catch (Exception ex)
            {
                return PipelineOutput.New.WithError(ex.Message);
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

                eventHandler.Handle(domainEvent, scope.ServiceProvider);
            }

            var commandQueue = scope.ServiceProvider.GetRequiredService<ICommandQueue>();

            commandQueue.Flush();

            return PipelineOutput.New
                .WithMessage("Command executed successfully")
                .WithResult(result);
        }
    }

    public PipelineOutput ExecuteCommand(ICommand command)
    {
        return ExecuteCommand((object)command);
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
                var returnType = ResolveCommandOutputType(commandType);

                var wrapperType = typeof(CommandHandlerWrapper<,>).MakeGenericType(commandType, returnType);

                var wrapperInstance = Activator.CreateInstance(wrapperType) ??
                                      throw new InvalidOperationException(
                                          $"Unable to create {nameof(CommandHandlerWrapper<ICommand, CommandOutput>)}<{commandType.Name}, {returnType.Name}> instance");

                return (CommandHandlerWrapper)wrapperInstance;
            });

            object? result;

            try
            {
                logger.LogDebug("Executing command: {CommandType}", command.GetType().Name);

                result = await commandHandler.HandleAsync(command, scope.ServiceProvider);

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

                return PipelineOutput.New.WithError(
                    "A retryable exception was thrown but the maximum retry count was reached");
            }
            catch (Exception ex)
            {
                return PipelineOutput.New.WithError(ex.Message);
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

            return PipelineOutput.New
                .WithMessage("Command executed successfully")
                .WithResult(result);
        }
    }

    public async Task<PipelineOutput> ExecuteCommandAsync(ICommand command)
    {
        return await ExecuteCommandAsync((object)command);
    }

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

        logger.LogDebug("Executing query: {QueryType}", query.GetType().Name);

        var result = await queryHandler.ExecuteAsync(query, scope.ServiceProvider);

        return PipelineOutput.New
            .WithMessage("Query executed successfully")
            .WithResult(result);
    }

    public Task<TResult> ExecuteQueryAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult> where TResult : class
    {
        return ExecuteQueryAsync(query) as Task<TResult> ?? Task.FromResult<TResult>(null!);
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

    private static Type ResolveCommandOutputType(Type commandType)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var type in assemblies.SelectMany(SafeGetTypes))
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // Reason: the null check is necessary
            if (type is null || !type.IsClass || type.IsAbstract)
            {
                continue;
            }

            // Prefer async handler if both exist
            var asyncInterface = type.GetInterfaces().FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(ICommandHandlerAsync<,>) &&
                i.GetGenericArguments()[0] == commandType);

            if (asyncInterface != null)
                return asyncInterface.GetGenericArguments()[1];

            var syncInterface = type.GetInterfaces().FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) &&
                i.GetGenericArguments()[0] == commandType);

            if (syncInterface != null)
                return syncInterface.GetGenericArguments()[1];
        }

        throw new InvalidOperationException(
            $"No command handler found to infer output type for command '{commandType.FullName}'.");

        static IEnumerable<Type> SafeGetTypes(Assembly assembly)
        {
            try { return assembly.GetTypes(); }
            catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null)!; }
        }
    }
}
