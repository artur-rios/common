namespace ArturRios.Common.Pipelines.Events.Interfaces;

public interface IDomainEventHandler<in TEvent> where TEvent : DomainEvent
{
    Task Handle(TEvent domainEvent);
}
