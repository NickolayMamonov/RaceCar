using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates;

public class SimulateRaceModel
{
    public required RaceId RaceId { get; init; }
}