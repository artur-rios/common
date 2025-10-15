namespace ArturRios.Common.Output;

public class DataOutput<T> : ProcessOutput
{

    // ReSharper disable once MemberCanBeProtected.Global
    // Reason: setter must be public for deserialization purposes
    public T? Data { get; set; }

    public static DataOutput<T> New => new();

    public DataOutput<T> WithData(T data)
    {
        Data = data;

        return this;
    }

    public DataOutput<T> WithError(string error)
    {
        AddError(error);

        return this;
    }

    public DataOutput<T> WithErrors(IEnumerable<string> errors)
    {
        AddErrors(errors);

        return this;
    }

    public DataOutput<T> WithMessage(string message)
    {
        AddMessage(message);

        return this;
    }

    public DataOutput<T> WithMessages(IEnumerable<string> messages)
    {
        AddMessages(messages);

        return this;
    }
}
