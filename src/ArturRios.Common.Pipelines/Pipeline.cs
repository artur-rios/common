using ArturRios.Common.Output;
using ArturRios.Common.Pipelines.Commands;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Queries;

namespace ArturRios.Common.Pipelines;

public class Pipeline(IServiceProvider serviceProvider)
{
    public TOutput ExecuteCommand<TCommand, TOutput>(TCommand command)
        where TCommand : Command
        where TOutput : ProcessOutput
    {
        var handler =
            (ICommandHandler<TCommand, TOutput>?)serviceProvider.GetService(typeof(ICommandHandler<TCommand, TOutput>));

        return handler is null
            ? throw new InvalidOperationException($"No handler registered for {typeof(TCommand).Name}")
            : handler.Handle(command);
    }

    public async Task<TOutput> ExecuteCommandAsync<TCommand, TOutput>(TCommand command)
        where TCommand : Command
        where TOutput : ProcessOutput
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

    public PaginatedOutput<TOutput?> ExecuteQuery<TQuery, TOutput>(TQuery query)
        where TQuery : Query
        where TOutput : QueryOutput
    {
        var handler =
            (IQueryHandler<TQuery, TOutput>?)serviceProvider.GetService(
                typeof(IQueryHandler<TQuery, TOutput>));

        return handler is null
            ? throw new InvalidOperationException($"No query handler registered for {typeof(TQuery).Name}")
            : handler.Handle(query);
    }

    public async Task<PaginatedOutput<TOutput?>> ExecuteQueryAsync<TQuery, TOutput>(TQuery query)
        where TQuery : Query
        where TOutput : QueryOutput
    {
        var handler =
            (IQueryHandlerAsync<TQuery, TOutput>?)serviceProvider.GetService(
                typeof(IQueryHandlerAsync<TQuery, TOutput>));

        if (handler == null)
        {
            throw new InvalidOperationException($"No async query handler registered for {typeof(TQuery).Name}");
        }

        return await handler.HandleAsync(query);
    }
}
