using ArturRios.Common.Pipelines.Commands;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Queries;

namespace ArturRios.Common.Pipelines;

public class Pipeline(IServiceProvider serviceProvider)
{
    public TOutput ExecuteCommand<TCommand, TOutput>(TCommand command)
        where TCommand : Command
        where TOutput : CommandOutput
    {
        var handler =
            (ICommandHandler<TCommand, TOutput>?)serviceProvider.GetService(typeof(ICommandHandler<TCommand, TOutput>));

        return handler is null
            ? throw new InvalidOperationException($"No handler registered for {typeof(TCommand).Name}")
            : handler.Handle(command);
    }

    public async Task<TOutput> ExecuteCommandAsync<TCommand, TOutput>(TCommand command)
        where TCommand : Command
        where TOutput : CommandOutput
    {
        var handler =
            (ICommandHandlerAsync<TCommand, TOutput>?)serviceProvider.GetService(
                typeof(ICommandHandlerAsync<TCommand, TOutput>));

        if (handler is null)
        {
            throw new InvalidOperationException($"No async handler registered for {typeof(TCommand).Name}");
        }

        return await handler.HandleAsync(command);
    }

    public TQueryOutput ExecuteQuery<TQuery, TQueryOutput>(TQuery query)
        where TQuery : Query
        where TQueryOutput : QueryOutput
    {
        var handler =
            (IQueryHandler<TQuery, TQueryOutput>?)serviceProvider.GetService(
                typeof(IQueryHandler<TQuery, TQueryOutput>));

        return handler is null
            ? throw new InvalidOperationException($"No query handler registered for {typeof(TQuery).Name}")
            : handler.Handle(query);
    }

    public async Task<TQueryOutput> ExecuteQueryAsync<TQuery, TQueryOutput>(TQuery query)
        where TQuery : Query
        where TQueryOutput : QueryOutput
    {
        var handler =
            (IQueryHandlerAsync<TQuery, TQueryOutput>?)serviceProvider.GetService(
                typeof(IQueryHandlerAsync<TQuery, TQueryOutput>));

        if (handler == null)
        {
            throw new InvalidOperationException($"No async query handler registered for {typeof(TQuery).Name}");
        }

        return await handler.HandleAsync(query);
    }
}
