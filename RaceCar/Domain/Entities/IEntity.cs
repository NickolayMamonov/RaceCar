using RaceCar.Domain.Common;

namespace RaceCar.Domain.Entities;

public interface IAggregate<T> : IAggregate, IEntity<T>
{
    
}

public interface IAggregate
{
    void AddDomainEvent(IDomainEvent domainEvent);
    IReadOnlyList<IDomainEvent> GetDomainEvents();
    IEvent[] ClearDomainEvents();
}


public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}

public interface IEntity
{
    public bool IsDeleted { get; set; }
}