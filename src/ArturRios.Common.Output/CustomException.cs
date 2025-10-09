namespace ArturRios.Common.Output;

public abstract class CustomException(string[] messages) : Exception(string.Join(", ", messages))
{
    public string[] Messages { get; } = messages;
}
