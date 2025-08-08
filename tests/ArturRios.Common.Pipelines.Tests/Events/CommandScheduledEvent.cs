using ArturRios.Common.Pipelines.Events;

namespace ArturRios.Common.Pipelines.Tests.Events;

public class CommandScheduledEvent : DomainEvent
{
    public required Guid OperationId { get; set; }
}
