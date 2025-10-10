namespace ArturRios.Common.Pipelines.Commands;

public abstract class CommandOutput
{
    public string[] Messages { get; set; } = [];
    public bool Success { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
