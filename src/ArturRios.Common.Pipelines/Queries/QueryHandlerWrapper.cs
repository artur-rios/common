using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Pipelines.Queries;

internal abstract class QueryHandlerWrapper
{
    public abstract Task<object> ExecuteAsync(object query, IServiceProvider serviceProvider);
}

internal class QueryHandlerWrapper<TQuery, TResult> : QueryHandlerWrapper
    where TQuery : IQuery<TResult> where TResult : class
{
    public override async Task<object> ExecuteAsync(object query, IServiceProvider serviceProvider)
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();

        return await handler.Execute((TQuery)query);
    }
}
