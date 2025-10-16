namespace ArturRios.Common.Output;

public class PaginatedOutput<T> : DataOutput<List<T>>
{
    public int PageNumber { get; set; }
    public int PageSize => Data?.Count ?? 0;
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public static new PaginatedOutput<T> New => new();

    public PaginatedOutput<T> WithPagination(int pageNumber, int totalItems)
    {
        PageNumber = pageNumber;
        TotalItems = totalItems;

        return this;
    }

    public new PaginatedOutput<T> WithData(List<T> data)
    {
        Data = data;

        return this;
    }

    public PaginatedOutput<T> WithEmptyData()
    {
        Data = [];

        return this;
    }

    public new PaginatedOutput<T> WithError(string error)
    {
        AddError(error);

        return this;
    }

    public new PaginatedOutput<T> WithErrors(IEnumerable<string> errors)
    {
        AddErrors(errors);

        return this;
    }

    public new PaginatedOutput<T> WithMessage(string message)
    {
        AddMessage(message);

        return this;
    }

    public new PaginatedOutput<T> WithMessages(IEnumerable<string> messages)
    {
        AddMessages(messages);

        return this;
    }
}
