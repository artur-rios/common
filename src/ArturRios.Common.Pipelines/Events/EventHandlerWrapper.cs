using ArturRios.Common.Extensions;
using ArturRios.Common.Pipelines.Events.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Pipelines.Events;

public abstract class EventHandlerWrapper
{
    public abstract Task Handle(object domainEvent, IServiceProvider serviceProvider);
}

public class EventHandlerWrapper<TEvent> : EventHandlerWrapper where TEvent : class
{
    public override async Task Handle(object domainEvent, IServiceProvider serviceProvider)
    {
        var logger = (ILogger)serviceProvider.GetService(typeof(ILogger<>).MakeGenericType(GetType()))!;
        var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();
        var eventHandlers = handlers as IEventHandler<TEvent>[] ?? handlers.ToArray();

        if (eventHandlers.IsNotEmpty())
        {
            foreach (var handler in eventHandlers)
            {
                logger.LogDebug("Executing event handler {HandlerName} for event {EventName}", handler.GetType().Name,
                    typeof(TEvent).Name);

                try
                {
                    await handler.Handle((TEvent)domainEvent);

                    logger.LogDebug("Event handler {HandlerName} executed successfully for event {EventName}",
                        handler.GetType().Name, typeof(TEvent).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error executing event handler {HandlerName} for event {EventName}",
                        handler.GetType().Name, typeof(TEvent).Name);
                }
            }
        }
    }
}
