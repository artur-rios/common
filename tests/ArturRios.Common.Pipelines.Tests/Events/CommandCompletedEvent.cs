using ArturRios.Common.Pipelines.Events;

namespace ArturRios.Common.Pipelines.Tests.Events;

public class CommandCompletedEvent : DomainEvent
{
    public required Guid OperationId { get; set; }
}
