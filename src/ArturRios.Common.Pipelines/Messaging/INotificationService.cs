namespace ArturRios.Common.Pipelines.Messaging;

public interface INotificationService
{
    Task PublishAsync<TNotification>(TNotification notification)
        where TNotification : Notification;
}
