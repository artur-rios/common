namespace ArturRios.Common.Pipelines.Queries;

public abstract class Query
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}
