using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Services;

public class RaceService : IRaceService
{
    private readonly RaceContext _context;
    private readonly IDriverService _driverService;

    public RaceService(RaceContext context, IDriverService driverService)
    {
        _context = context;
        _driverService = driverService;
    }
    public async Task<Race> CreateRace(Label raceName)
    {
        var drivers = await _context.Drivers.ToListAsync();

        var selectedDrivers = SelectDrivers(drivers);

        if (selectedDrivers.Count < 2)
        {
            throw new Exception("There are not enough suitable drivers available.");
        }

        var race = Race.Create(RaceId.Of(Guid.NewGuid()), raceName, selectedDrivers.Select(d => d.Id).ToList());

        foreach (var driver in selectedDrivers)
        {
            driver.SetRaceId(race.Id);
        }
        await _context.Races.AddAsync(race);
        await _context.SaveChangesAsync();

        return race;
    }

   
    private List<Driver> SelectDrivers(List<Driver> drivers)
    {
        var selectedDrivers = new List<Driver>();

        foreach (var driver in drivers)
        {
            var similarDrivers = drivers.Where(d =>
                d.CarType == driver.CarType &&
                Math.Abs(d.HorsePower - driver.HorsePower) <= 100 &&
                d != driver &&
                !selectedDrivers.Contains(d)
            ).ToList();

            if (similarDrivers.Any())
            {
                selectedDrivers.Add(driver);
                selectedDrivers.Add(similarDrivers.First());
                break;
            }
        }

        return selectedDrivers;
    }
    
    public async Task<Race> SimulateRace(RaceId raceId)
    {
        var race = await _context.Races.FindAsync(raceId);

        if (race == null)
        {
            throw new Exception("Race not found.");
        }

        var drivers = await _context.Drivers.Where(d => d.RaceId == raceId).ToListAsync();

        Random random = new Random();
        var winner = drivers[random.Next(0, drivers.Count)];

        race.SetWinner(winner);

        await _context.SaveChangesAsync();

        return race;
    }
}
