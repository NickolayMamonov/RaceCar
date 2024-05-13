using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.Services;

public interface IRaceService
{
    Task<RaceDto> CreateRaceAsync(Label raceName);
    Task<List<RaceDto>> GetAllRacesAsync();
    Task<Race> SimulateRace(RaceId raceId);
}