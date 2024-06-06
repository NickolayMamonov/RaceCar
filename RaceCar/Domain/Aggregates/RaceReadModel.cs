using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates;

public class RaceReadModel
{
    public required Guid Id { get; init; }
    public required Label Label { get; init; }
    public required TypeOfCar TypeOfCar { get; init; }
    public required List<Guid> Drivers { get; init; }
    public required DateTime CreatedAt { get; init; }
}