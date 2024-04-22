using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Services;

public class DriverService: IDriverService
{
    private readonly RaceContext _context;

    public DriverService(RaceContext context)
    {
        _context = context;
    }

    public async Task<Driver> AddDriverAsync(Name name, CarType carType, HorsePower horsePower)
    {
        var driver = Driver.Create(DriverId.Of(Guid.NewGuid()), name, carType, horsePower);
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();
        return driver;
    }

}