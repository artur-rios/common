// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// Reason: Keep the setters for deserialization purposes.

// ReSharper disable MemberCanBeProtected.Global
// Reason: public empty constructor is necessary for json deserialization

namespace ArturRios.Common.Output;

public class DataOutput<T>
{
    public DataOutput()
    {
    }

    public DataOutput(T? data, string[] messages, bool success)
    {
        Data = data;
        Messages = messages;
        Success = success;
    }

    public T? Data { get; set; }
    public string[] Messages { get; set; } = [];
    public bool Success { get; set; }

    // ReSharper disable once UnusedMember.Global
    // Reason: This is a metadata field
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
