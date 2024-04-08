using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Services;

public class RaceService : IRaceService
{
    private readonly RaceContext _context;

    public RaceService(RaceContext context)
    {
        _context = context;
    }
    public async Task<Race> CreateRace(string raceName, List<Driver> drivers)
    {
        if (drivers.Count < 2)
        {
            throw new ArgumentException("At least two drivers are required for a race.");
        }
        var selectedDrivers = SelectDrivers(drivers);
        // Create Race
        var race = Race.Create(RaceId.Of(Guid.NewGuid()), raceName, selectedDrivers);

        // Save to database
        await _context.Races.AddAsync(race);
        await _context.SaveChangesAsync();
        foreach (var driver in selectedDrivers)
        {
            driver.SetRaceId(race.Id); // Assuming you have a method to set RaceId in Driver class
        }
        // Simulate the race and determine the winner
        var winner = SimulateRace(selectedDrivers);

        // Set winner
        race.SetWinner(winner);

        // Update race in the database
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

    private Driver SimulateRace(List<Driver> drivers)
    {
        // Simulate race logic here, for example, just randomly select a winner
        Random random = new Random();
        return drivers[random.Next(0, 2)]; // randomly select one of the drivers as winner
    }
   

}
 // public async Task<Race> CreateRace(string raceLabel)
    // {
    //     // Retrieve drivers from the database based on criteria
    //     var drivers = await _context.Drivers
    //         .Where(d => !_context.Races.Any(r => r.Drivers.Contains(d)))
    //         .ToListAsync(); // assuming you have DbSet<Driver> in your context
    //
    //     // Select two drivers with similar CarType and HorsePower difference not greater than 100
    //     var selectedDrivers = SelectDrivers(drivers);
    //
    //     if (selectedDrivers.Count < 2)
    //     {
    //         throw new InvalidOperationException("Unable to find suitable drivers for the race.");
    //     }
    //
    //     // Simulate the race and determine the winner
    //     var winner = SimulateRace(selectedDrivers);
    //
    //     // Create Race
    //     var race = Race.Create(RaceId.Of(Guid.NewGuid()), raceLabel, selectedDrivers);
    //
    //     // Set winner
    //     race.SetWinner(winner);
    //
    //     // Save to database
    //     await _context.Races.AddAsync(race);
    //     await _context.SaveChangesAsync();
    //
    //     return race;
    // }
    //
    // private List<Driver> SelectDrivers(List<Driver> drivers)
    // {
    //     var selectedDrivers = new List<Driver>();
    //
    //     foreach (var driver in drivers)
    //     {
    //         var similarDrivers = drivers.Where(d =>
    //             d.CarType == driver.CarType &&
    //             Math.Abs(d.HorsePower - driver.HorsePower) <= 100 &&
    //             d != driver &&
    //             !selectedDrivers.Contains(d)
    //         ).ToList();
    //
    //         if (similarDrivers.Any())
    //         {
    //             selectedDrivers.Add(driver);
    //             selectedDrivers.Add(similarDrivers.First());
    //             break;
    //         }
    //     }
    //
    //     return selectedDrivers;
    // }
    //
    // private Driver SimulateRace(List<Driver> drivers)
    // {
    //     // Simulate race logic here, for example, just randomly select a winner
    //     Random random = new Random();
    //     return drivers[random.Next(0, 2)]; // randomly select one of the drivers as winner
    // }
