using RaceCar.Domain.Common;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates.Events;

public record RaceCreatedDomainEvent(RaceId Id, string Label,string TypeOfCar,string? Winner, List<DriverId> Drivers) : IDomainEvent;