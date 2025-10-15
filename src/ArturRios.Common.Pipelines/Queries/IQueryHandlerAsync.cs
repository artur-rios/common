namespace ArturRios.Common.Pipelines.Queries;

public interface IQueryHandlerAsync<in TQuery, TOutput> where TQuery : Query where TOutput : QueryOutput
{
    Task<TOutput> HandleAsync(TQuery query);
}
