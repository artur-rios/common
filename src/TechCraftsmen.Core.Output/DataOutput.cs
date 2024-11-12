namespace TechCraftsmen.Core.Output;

public class DataOutput<T>(T? data, string[] messages, bool success)
{
    public T? Data { get; } = data;
    public string[] Messages { get; } = messages;
    public bool Success = success;
    public DateTime Timestamp = DateTime.UtcNow;
}
