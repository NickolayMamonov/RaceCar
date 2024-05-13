using RaceCar.Domain.Common;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates.Events;

public record RaceCreatedDomainEvent
    (RaceId Id, string Label, List<DriverId> Drivers, DateTime CreatedAt) : IDomainEvent;