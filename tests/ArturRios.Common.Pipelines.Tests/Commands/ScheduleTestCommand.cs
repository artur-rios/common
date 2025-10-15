using ArturRios.Common.Pipelines.Commands;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class ScheduleTestCommand(int id, DateTime scheduleDate) : Command
{
    public int Id { get; set; } = id;
    public DateTime ScheduledDate { get; set; } = scheduleDate;
}
