namespace RaceCar.Domain.Entities;

public interface IAggregate<T> : IAggregate, IEntity<T>
{
}

public interface IAggregate : IEntity
{
}


public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}

public interface IEntity
{
    public bool IsDeleted { get; set; }
}