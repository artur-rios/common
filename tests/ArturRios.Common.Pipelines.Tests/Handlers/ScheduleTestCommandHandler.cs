using ArturRios.Common.Data.Interfaces;
using ArturRios.Common.Output;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Tests.Commands;
using ArturRios.Common.Pipelines.Tests.Entities;
using ArturRios.Common.Pipelines.Tests.Output;

namespace ArturRios.Common.Pipelines.Tests.Handlers;

public class ScheduleTestCommandHandler(ICommandQueue commandQueue, ICrudRepository<TestEntity> repository) : ICommandHandlerAsync<ScheduleTestCommand, ScheduleTestCommandOutput>
{

    public Task<DataOutput<ScheduleTestCommandOutput>> HandleAsync(ScheduleTestCommand command)
    {
        var entity = repository.GetById(command.Id);

        if (entity is null)
        {
            throw new ArgumentException($"Entity with Id {command.Id} not found");
        }

        var scheduleDate = command.ScheduledDate.AddDays(1);

        if (IsWeekendDay(scheduleDate))
        {
            scheduleDate = GetNextWeekday(scheduleDate);
        }

        entity.CanSchedule(scheduleDate);

        commandQueue.Schedule(new ScheduledTestCommand(entity.Id), scheduleDate);

        entity.MarkAsScheduled();

        var output = DataOutput<ScheduleTestCommandOutput>.New
            .WithData(new ScheduleTestCommandOutput())
            .WithMessage("Command scheduled successfully");

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
