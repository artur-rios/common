namespace ArturRios.Common.Pipelines.Queries;

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> where TResult : class?
{
    Task<TResult> Execute(TQuery query);
}
