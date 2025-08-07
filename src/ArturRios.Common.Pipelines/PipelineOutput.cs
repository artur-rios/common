namespace ArturRios.Common.Pipelines;

public class PipelineOutput
{
    public string[] Messages { get; set; } = [];
    public bool Success { get; set; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

public class PipelineOutput<T>
{
    public T? Data { get; set; }
    public string[] Messages { get; set; } = [];
    public bool Success { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
