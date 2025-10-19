namespace ArturRios.Common.Output;

public class DataOutput<T> : ProcessOutput
{
    public T? Data { get; set; }

    public static new DataOutput<T> New => new();

    public DataOutput<T> WithData(T data)
    {
        Data = data;

        return this;
    }

    public new DataOutput<T> WithError(string error)
    {
        AddError(error);

        return this;
    }

    public new DataOutput<T> WithErrors(IEnumerable<string> errors)
    {
        AddErrors(errors);

        return this;
    }

    public new DataOutput<T> WithMessage(string message)
    {
        AddMessage(message);

        return this;
    }

    public new DataOutput<T> WithMessages(IEnumerable<string> messages)
    {
        AddMessages(messages);

        return this;
    }
}
