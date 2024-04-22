using RaceCar.Domain.Common;

namespace RaceCar.Domain.Entities;

public class Entity<T>
{
    public T Id { get; set; }
    public bool IsDeleted { get; set; }
    // public Guid Id { get; protected set; }
    // public override bool Equals(object obj)
    // {
    //     if (obj is not Entity other)
    //     {
    //         return false;
    //     }
    //
    //     if (ReferenceEquals(this, other))
    //     {
    //         return true;
    //     }
    //
    //     if (Id.Equals(default) || other.Id.Equals(default))
    //     {
    //         return false;
    //     }
    //
    //     return Id.Equals(other.Id);
    // }
    //
    // public static bool operator ==(Entity a, Entity b)
    //     => a is null && b is null || a is not null && b is not null && a.Equals(b);
    //
    // public static bool operator !=(Entity a, Entity b)
    //     => !(a == b);
    //
    // public override int GetHashCode()
    //     => (GetType().ToString() + Id).GetHashCode(); 
}
public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    public IEvent[] ClearDomainEvents()
    {
        IEvent[] dequeuedEvents = _domainEvents.ToArray();

        _domainEvents.Clear();

        return dequeuedEvents;
    }
}