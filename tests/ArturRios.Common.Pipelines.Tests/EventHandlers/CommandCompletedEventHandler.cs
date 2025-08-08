using ArturRios.Common.Aws.Sns;
using ArturRios.Common.Pipelines.Events.Interfaces;
using ArturRios.Common.Pipelines.Tests.Events;
using ArturRios.Common.Pipelines.Tests.Notifications;

namespace ArturRios.Common.Pipelines.Tests.EventHandlers;

public class CommandCompletedEventHandler(ISnsService notificationService) : IEventHandler<CommandCompletedEvent>
{
    public async Task Handle(CommandCompletedEvent @event)
    {
        var notification = new CommandCompletedNotification(@event.OperationId);

        await notificationService.PublishAsync(notification);
    }
}
