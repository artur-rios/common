using System.Collections.Concurrent;
using ArturRios.Common.Extensions;
using ArturRios.Common.Pipelines.Events.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Pipelines.Events;

public class DomainEventBus(ILogger<DomainEventBus> logger, IServiceProvider serviceProvider)
    : IDomainEventBus
{
    private static readonly ConcurrentDictionary<Type, DomainEventHandlerWrapper> s_wrappers = new();
    private readonly List<DomainEvent> _domainEvents = [];

    public DomainEvent[] DomainEvents => _domainEvents.ToArray();

    public async Task Dispatch<TContext>(TContext dbContext) where TContext : DbContext
    {
        var generation = 0;
        var cleared = false;
        var total = 0;

        while (!cleared)
        {
            logger.LogDebug("Dispatching domain events of generation {Generation}...", generation);

            cleared = true;

            var entities = dbContext.ChangeTracker.Entries<DomainEventEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.IsNotEmpty())
                .ToArray();

            foreach (var entity in entities)
            {
                var events = entity.DomainEvents?.ToArray();

                if (events.IsEmpty())
                {
                    continue;
                }

                cleared = false;

                entity.DomainEvents!.Clear();

                foreach (var domainEvent in events!)
                {
                    logger.LogDebug("Publishing event {EventType}: {EventData}", domainEvent.GetType(), domainEvent);

                    await Publish(domainEvent);

                    total++;

                    _domainEvents.Add(domainEvent);
                }
            }

            generation++;

            if (generation >= 5)
            {
                throw new InvalidOperationException("Max number of iterations reached when processing domain events");
            }
        }

        logger.LogDebug("Finished dispatching domain events. Total events dispatched: {Total}", total);
    }

    public async Task Publish(DomainEvent domainEvent)
    {
        var wrapper = s_wrappers.GetOrAdd(domainEvent.GetType(), static requestType =>
        {
            var wrapperType = typeof(DomainEventHandlerWrapper<>).MakeGenericType(requestType);
            var wrapperInstance = Activator.CreateInstance(wrapperType) ??
                                  throw new InvalidOperationException(
                                      $"Could not create instance of {nameof(DomainEventHandlerWrapper<DomainEvent>)} for type {requestType}");

            return (DomainEventHandlerWrapper)wrapperInstance;
        });

        await wrapper.Handle(domainEvent, serviceProvider);
    }
}
