namespace ArturRios.Common.Output;

public class PaginatedOutput<T> : DataOutput<List<T>>
{
    public int PageNumber { get; set; }
    public int PageSize => Data?.Count ?? 0;
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}
