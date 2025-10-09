using ArturRios.Common.Pipelines.Events;
using ArturRios.Common.Pipelines.Tests.Events;
using ArturRios.Common.Util.Condition;

namespace ArturRios.Common.Pipelines.Tests.Entities;

public class TestEntity : DomainEventEntity
{
    public Guid OperationId { get; set; }
    public string Data { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public bool Scheduled { get; set; }
    public bool Completed { get; set; }

    private void CanDoSomething()
    {
        Condition.Create
            .False(!string.IsNullOrWhiteSpace(Data))
            .FailsWith("Data cannot be empty")
            .ThrowIfNotSatisfied();
    }

    public void DoSomething()
    {
        CanDoSomething();

        AddDomainEvent(new TestEvent { OperationId = OperationId, ScheduleDate = CreatedAt });
    }

    public void CanSchedule(DateTime scheduleDate)
    {
        Condition.Create
            .True(scheduleDate > DateTime.UtcNow)
            .FailsWith("Schedule date must be in the future")
            .True(scheduleDate > CreatedAt)
            .FailsWith("Schedule date must be after entity creation date")
            .ThrowIfNotSatisfied();
    }

    public void MarkAsScheduled()
    {
        Scheduled = true;

        AddDomainEvent(new CommandScheduledEvent { OperationId = OperationId });
    }

    private void CanComplete()
    {
        Condition.Create
            .True(Scheduled)
            .FailsWith("Entity must be scheduled before completing")
            .ThrowIfNotSatisfied();
    }

    public void MarkAsCompleted()
    {
        CanComplete();
        Completed = true;

        AddDomainEvent(new CommandCompletedEvent { OperationId = OperationId });
    }
}
