using ArturRios.Common.Pipelines.Events.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Pipelines.Events;

internal abstract class DomainEventHandlerWrapper
{
    public abstract Task Handle(DomainEvent domainEvent, IServiceProvider serviceProvider);
}

internal class DomainEventHandlerWrapper<TEvent> : DomainEventHandlerWrapper where TEvent : DomainEvent
{
    public override async Task Handle(DomainEvent domainEvent, IServiceProvider serviceProvider)
    {
        var handlers = serviceProvider.GetServices<IDomainEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            await handler.Handle((TEvent)domainEvent);
        }
    }
}
