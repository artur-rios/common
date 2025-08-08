using ArturRios.Common.Pipelines.Messaging;

namespace ArturRios.Common.Pipelines.Tests.Notifications;

public class CommandScheduledNotification(Guid operationId) : Notification
{
    public Guid OperationId { get; set; } = operationId;
}
