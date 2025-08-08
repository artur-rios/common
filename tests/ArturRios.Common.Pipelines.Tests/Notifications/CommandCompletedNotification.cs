using ArturRios.Common.Pipelines.Messaging;

namespace ArturRios.Common.Pipelines.Tests.Notifications;

public class CommandCompletedNotification(Guid operationId) : Notification
{
    public Guid OperationId { get; set; } = operationId;
}
