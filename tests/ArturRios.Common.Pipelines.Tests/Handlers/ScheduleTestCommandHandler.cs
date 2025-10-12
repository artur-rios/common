using ArturRios.Common.Data.Interfaces;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Tests.Commands;
using ArturRios.Common.Pipelines.Tests.Entities;
using ArturRios.Common.Pipelines.Tests.Filters;
using ArturRios.Common.Pipelines.Tests.Output;

namespace ArturRios.Common.Pipelines.Tests.Handlers;

public class ScheduleTestCommandHandler(ICommandQueue commandQueue, ICrudRepository<TestEntity> repository) : ICommandHandlerAsync<ScheduleTestCommand, ScheduleTestCommandOutput>
{

    public Task<ScheduleTestCommandOutput> HandleAsync(ScheduleTestCommand command)
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

        commandQueue.Schedule(new ScheduledTestCommand(entity.OperationId), scheduleDate);

        entity.MarkAsScheduled();

        var output = new ScheduleTestCommandOutput();
        output.AddMessage("Command scheduled successfully");

        return Task.FromResult(output);
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
