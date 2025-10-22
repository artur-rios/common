using ArturRios.Common.Data;

namespace ArturRios.Common.Pipelines.Events;

public class DomainEventEntity : Entity
{
    public List<DomainEvent>? DomainEvents { get; private set; }

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        DomainEvents ??= [];
        DomainEvents.Add(domainEvent);
    }

    protected void RemoveDomainEvent(DomainEvent domainEvent) => DomainEvents?.Remove(domainEvent);

    public override bool Equals(object? obj) =>
        obj is DomainEventEntity entity &&
        obj.GetType() == GetType() &&
        Id.Equals(entity.Id);

    public override int GetHashCode() => HashCode.Combine(Id);

    public static bool operator ==(DomainEventEntity? left, DomainEventEntity? right) =>
        EqualityComparer<DomainEventEntity>.Default.Equals(left, right);

    public static bool operator !=(DomainEventEntity? left, DomainEventEntity? right) => !(left == right);
}
