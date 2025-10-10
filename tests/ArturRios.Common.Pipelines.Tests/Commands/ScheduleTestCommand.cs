using ArturRios.Common.Pipelines.Commands.Interfaces;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class ScheduleTestCommand(Guid operationId, DateTime scheduleDate) : ICommand
{
    public Guid OperationId { get; set; } = operationId;
    public DateTime ScheduledDate { get; set; } = scheduleDate;
}
