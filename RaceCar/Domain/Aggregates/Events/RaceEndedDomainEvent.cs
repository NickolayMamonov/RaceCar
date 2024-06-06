using RaceCar.Domain.Common;

namespace RaceCar.Domain.Aggregates.Events;

public record RaceEndedDomainEvent(Guid Id,string TypeOfCar, DateTime EndedAt, Guid WinnerId) : IDomainEvent;