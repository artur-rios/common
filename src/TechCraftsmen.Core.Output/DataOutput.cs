namespace TechCraftsmen.Core.Output;

public class DataOutput<T>
{
    // Necessary for json serialization
    public DataOutput()
    {
    }
    
    public DataOutput(T? data, string[] messages, bool success)
    {
        Data = data;
        Messages = messages;
        Success = success;
    }
    
    public T? Data { get; }
    public string[] Messages { get; } = [];
    public bool Success { get; }
    public DateTime Timestamp = DateTime.UtcNow;
}
