using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Queries;

public interface ISingleQueryHandlerAsync<in TQuery, TOutput>
    where TQuery : Query
    where TOutput : QueryOutput
{
    Task<DataOutput<TOutput?>> HandleAsync(TQuery query);
}
