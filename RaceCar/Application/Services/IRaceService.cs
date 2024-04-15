using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.Services;

public interface IRaceService
{
    Task<Race> CreateRace(Label raceLabel);
    Task<Race> SimulateRace(RaceId raceId);
}