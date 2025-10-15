namespace ArturRios.Common.Pipelines.Queries;

public interface IQueryHandler<in TQuery, out TOutput> where TQuery : Query where TOutput : QueryOutput
{
    TOutput Handle(TQuery query);
}
