using ArturRios.Common.Pipelines.Events;

namespace ArturRios.Common.Pipelines.Tests.Events;

public class TestEvent : DomainEvent
{
    public required int Id { get; set; }
    public required DateTime ScheduleDate { get; set; }
}
