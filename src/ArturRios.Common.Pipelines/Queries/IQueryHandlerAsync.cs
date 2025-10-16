using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Queries;

public interface IQueryHandlerAsync<in TQuery, TOutput> where TQuery : Query where TOutput : QueryOutput
{
    Task<PaginatedOutput<TOutput>> HandleAsync(TQuery query);
}
