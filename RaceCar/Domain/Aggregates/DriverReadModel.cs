using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates;

public class DriverReadModel
{
    public required Name Name { get; init; }
    public required CarType CarType { get; init; }
    public required HorsePower HorsePower { get; init; }
}