namespace ArturRios.Common.Output;

public abstract class CustomException : Exception
{
    public string[] Messages { get; } = [];

    protected CustomException(string[] messages) : base(string.Join(", ", messages))
    {
    }

    protected CustomException(string[] messages, string message) : base(message)
    {
        Messages = messages;
    }
}
