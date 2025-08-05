namespace ArturRios.Common.Pipelines.Commands.IO;

public class CommandOutput
{
    public string[] Messages { get; set; } = [];
    public bool Success { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class CommandOutput<T>
{
    public T? Data { get; set; }
    public string[] Messages { get; set; } = [];
    public bool Success { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
