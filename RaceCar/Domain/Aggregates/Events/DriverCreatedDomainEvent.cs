using RaceCar.Domain.Common;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates.Events;

public record DriverCreatedDomainEvent(Guid Id, Name Name, CarType CarType, HorsePower HorsePower) : IDomainEvent;