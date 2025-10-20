using ArturRios.Common.Output;
using ArturRios.Common.Pipelines.Commands;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Pipelines;

public class Pipeline(IServiceScopeFactory scopeFactory)
{
    public DataOutput<TOutput?> ExecuteCommand<TCommand, TOutput>(TCommand command)
        where TCommand : Command
        where TOutput : CommandOutput
    {
        using var scoped = scopeFactory.CreateScope();

        var handler = scoped.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TOutput>>();

        return handler.Handle(command);
    }

    public async Task<DataOutput<TOutput?>> ExecuteCommandAsync<TCommand, TOutput>(TCommand command)
        where TCommand : Command
        where TOutput : CommandOutput
    {
        using var scoped = scopeFactory.CreateScope();

        var handler = scoped.ServiceProvider.GetRequiredService<ICommandHandlerAsync<TCommand, TOutput>>();

        return await handler.HandleAsync(command);
    }

    public PaginatedOutput<TOutput> ExecuteQuery<TQuery, TOutput>(TQuery query)
        where TQuery : Query
        where TOutput : QueryOutput
    {
        using var scoped = scopeFactory.CreateScope();

        var handler = scoped.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TOutput>>();

        return handler.Handle(query);
    }

    public async Task<PaginatedOutput<TOutput>> ExecuteQueryAsync<TQuery, TOutput>(TQuery query)
        where TQuery : Query
        where TOutput : QueryOutput
    {
        using var scoped = scopeFactory.CreateScope();

        var handler = scoped.ServiceProvider.GetRequiredService<IQueryHandlerAsync<TQuery, TOutput>>();

        return await handler.HandleAsync(query);
    }

    public DataOutput<TOutput?> ExecuteSingleQuery<TQuery, TOutput>(TQuery query)
        where TQuery : Query
        where TOutput : QueryOutput
    {
        using var scoped = scopeFactory.CreateScope();

        var handler = scoped.ServiceProvider.GetRequiredService<ISingleQueryHandler<TQuery, TOutput>>();

        return handler.Handle(query);
    }

    public async Task<DataOutput<TOutput?>> ExecuteSingleQueryAsync<TQuery, TOutput>(TQuery query)
        where TQuery : Query
        where TOutput : QueryOutput
    {
        using var scoped = scopeFactory.CreateScope();

        var handler = scoped.ServiceProvider.GetRequiredService<ISingleQueryHandlerAsync<TQuery, TOutput>>();

        return await handler.HandleAsync(query);
    }

    private TService GetService<TService>() where TService : notnull
    {
        var scope = scopeFactory.CreateScope();

        return new ScopedService<TService>(scope, scope.ServiceProvider.GetRequiredService<TService>()).Service;
    }

    private sealed class ScopedService<T>(IServiceScope scope, T service) : IDisposable
    {
        public T Service { get; } = service;
        public void Dispose() => scope.Dispose();
    }
}
