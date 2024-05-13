using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.Aggregates.Events;
using RaceCar.Domain.ValueObjects;
using RaceCar.Features;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Services;

public class RaceService : IRaceService
{
    private readonly RaceContext _context;
    private readonly IDriverService _driverService;
    private readonly IMediator _mediator;

    public RaceService(RaceContext context, IDriverService driverService,IMediator mediator)
    {
        _context = context;
        _driverService = driverService;
        _mediator = mediator;
    }
  
    public async Task<RaceDto> CreateRaceAsync(Label raceName)
    {
        // var drivers = await _context.Drivers.ToListAsync();
        var getAllDriversResult = await _mediator.Send(new GetAllDrivers.GetAllDriversQuery());
        var drivers = getAllDriversResult.Drivers.Select(d => Driver.Create(DriverId.Of(d.Id), Name.Of(d.Name), CarType.Of(d.CarType), HorsePower.Of(d.HorsePower))).ToList();
        var selectedDrivers = SelectDrivers(drivers);

        if (selectedDrivers.Count > 8)
        {
            await _mediator.Publish(new RaceDriversFilledDomainEvent(selectedDrivers.Select(d=>d.Id).ToList(), DateTime.UtcNow));

        }
        else
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
        await _mediator.Publish(new RaceCreatedDomainEvent(race.Id, race.Label,selectedDrivers.Select(d => d.Id).ToList(), DateTime.UtcNow));
        
        return new RaceDto(
            race.Id.Value, 
            race.Label,
            selectedDrivers.Select(d => d.Id).ToList()
        );
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
                if (selectedDrivers.Count >= 8)
                {
                    break;
                }
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
