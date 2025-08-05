using Microsoft.EntityFrameworkCore;

namespace ArturRios.Common.Pipelines.Events.Interfaces;

public interface IDomainEventBus
{
    DomainEvent[] DomainEvents { get; }

    Task Dispatch<TContext>(TContext dbContext) where TContext : DbContext;
    Task Publish(DomainEvent domainEvent);
}
