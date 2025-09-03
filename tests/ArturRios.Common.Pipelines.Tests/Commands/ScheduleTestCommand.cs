using ArturRios.Common.Data;
using ArturRios.Common.Data.Interfaces;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Commands.IO;
using ArturRios.Common.Pipelines.Tests.Entities;
using ArturRios.Common.Pipelines.Tests.Filters;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class ScheduleTestCommand(ICommandQueue commandQueue, ICrudRepository<TestEntity> repository) : ICommandHandler<ScheduleTestCommand.Input, CommandOutput>
{
    public class Input(Guid operationId, DateTime scheduleDate) : ICommand<CommandOutput>
    {
        public Guid OperationId { get; set; } = operationId;
        public DateTime ScheduledDate { get; set; } = scheduleDate;
    }

    public Task<CommandOutput> HandleAsync(Input command)
    {
        var entity = repository.GetByFilter(new TestFilter { OperationId = command.OperationId }).FirstOrDefault();

        if (entity is null)
        {
            throw new ArgumentException($"Entity with OperationId {command.OperationId} not found");
        }

        var scheduleDate = command.ScheduledDate.AddDays(1);

        if (IsWeekendDay(scheduleDate))
        {
            scheduleDate = GetNextWeekday(scheduleDate);
        }

        entity.CanSchedule(scheduleDate);

        commandQueue.Schedule(new ScheduledTestCommand.Input(entity.OperationId), scheduleDate);

        entity.MarkAsScheduled();

        return Task.FromResult(new CommandOutput { Messages = ["Command scheduled successfully"], Success = true, });
    }

    private static bool IsWeekendDay(DateTime date)
    {
        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    private static DateTime GetNextWeekday(DateTime date)
    {
        var nextDay = date.AddDays(1);

        while (IsWeekendDay(nextDay))
        {
            nextDay = nextDay.AddDays(1);
        }

        return nextDay;
    }
}
