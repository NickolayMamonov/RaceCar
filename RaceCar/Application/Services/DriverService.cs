using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Services;

public class DriverService : IDriverService
{
    private readonly RaceContext _context;

    public DriverService(RaceContext context)
    {
        _context = context;
    }

    public async Task<DriverDto> AddDriverAsync(Name name, CarType carType, HorsePower horsePower)
    {
        var driver = Driver.Create(DriverId.Of(Guid.NewGuid()), name, carType, horsePower);
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();

        return new DriverDto(driver.Id.Value, driver.Name.Value, driver.CarType.Value, driver.HorsePower.Value,
            driver.RaceId ?? Guid.Empty);
    }

    public async Task<List<DriverDto>> GetAllDriversAsync()
    {
        var drivers = await _context.Drivers.ToListAsync();

        return drivers.Select(d =>
                new DriverDto(d.Id.Value, d.Name.Value, d.CarType.Value, d.HorsePower.Value, d.RaceId ?? Guid.Empty))
            .ToList();
    }
}