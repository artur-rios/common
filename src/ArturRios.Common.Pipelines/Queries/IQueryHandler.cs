using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Queries;

public interface IQueryHandler<in TQuery, TOutput> where TQuery : Query where TOutput : QueryOutput
{
    PaginatedOutput<TOutput> Handle(TQuery query);
}
