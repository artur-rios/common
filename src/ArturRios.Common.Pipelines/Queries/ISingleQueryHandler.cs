using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Queries;

public interface ISingleQueryHandler<in TQuery, TOutput>
    where TQuery : Query
    where TOutput : QueryOutput
{
    DataOutput<TOutput?> Handle(TQuery query);
}
