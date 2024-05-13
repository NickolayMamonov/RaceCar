using RaceCar.Domain.Common;

namespace RaceCar.Domain.Aggregates.Events;

public record RaceEndedDomainEvent(Guid Id, DateTime EndedAt, Guid WinnerId) : IDomainEvent;