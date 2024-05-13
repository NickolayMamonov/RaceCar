using RaceCar.Domain.Common;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates.Events;

public record RaceDriversFilledDomainEvent(List<DriverId> Drivers, DateTime FilledAt) : IDomainEvent;