namespace ArturRios.Common.Pipelines.Events;

public class DomainEvent
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
