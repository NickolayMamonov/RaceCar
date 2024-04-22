namespace RaceCar.Domain.Aggregates;

public class DriverReadModel
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string CarType { get; init; }
    public required int HorsePower { get; init; }
}