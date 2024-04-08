using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates;

namespace RaceCar.Application.Services;

public interface IRaceService
{
    Task<Race> CreateRace(string raceLabel, List<Driver> drivers);
}