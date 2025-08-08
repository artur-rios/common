using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Events.Interfaces;
using ArturRios.Common.Pipelines.Tests.Commands;
using ArturRios.Common.Pipelines.Tests.Events;

namespace ArturRios.Common.Pipelines.Tests.EventHandlers;

public class TestEventHandler(ICommandQueue commandQueue) : IEventHandler<TestEvent>
{
    public Task Handle(TestEvent @event)
    {
        commandQueue.Enqueue(new ScheduleTestCommand.Input(@event.OperationId, @event.ScheduleDate));

        return Task.CompletedTask;
    }
}
