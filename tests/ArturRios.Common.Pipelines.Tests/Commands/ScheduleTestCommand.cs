using ArturRios.Common.Pipelines.Commands;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class ScheduleTestCommand(Guid operationId, DateTime scheduleDate) : Command
{
    public Guid OperationId { get; set; } = operationId;
    public DateTime ScheduledDate { get; set; } = scheduleDate;
}
