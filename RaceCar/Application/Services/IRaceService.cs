using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.Services;

public interface IRaceService
{
    Task<RaceDto> CreateRaceAsync(Label raceName);
    Task<Race> SimulateRace(RaceId raceId);
}