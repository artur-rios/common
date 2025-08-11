namespace ArturRios.Common.Pipelines.Queries;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult> where TResult : class?
{
    Task<TResult> ExecuteAsync(TQuery query);
}
