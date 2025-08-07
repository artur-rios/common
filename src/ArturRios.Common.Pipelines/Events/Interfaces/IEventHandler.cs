namespace ArturRios.Common.Pipelines.Events.Interfaces;

public interface IEventHandler<in TEvent> where TEvent : class
{
    Task Handle(TEvent @event);
}
