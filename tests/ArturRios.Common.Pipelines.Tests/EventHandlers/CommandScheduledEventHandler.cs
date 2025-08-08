using ArturRios.Common.Aws.Sns;
using ArturRios.Common.Pipelines.Events.Interfaces;
using ArturRios.Common.Pipelines.Tests.Events;
using ArturRios.Common.Pipelines.Tests.Notifications;

namespace ArturRios.Common.Pipelines.Tests.EventHandlers;

public class CommandScheduledEventHandler(ISnsService notificationService) : IEventHandler<CommandScheduledEvent>
{
    public async Task Handle(CommandScheduledEvent @event)
    {
        var notification = new CommandScheduledNotification(@event.OperationId);

        await notificationService.PublishAsync(notification);
    }
}
