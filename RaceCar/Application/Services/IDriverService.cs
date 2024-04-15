using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.Services;

public interface IDriverService
{
    Task<Driver> AddDriverAsync(Name name, CarType carType, HorsePower horsePower);
}