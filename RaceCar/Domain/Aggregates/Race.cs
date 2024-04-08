using RaceCar.Domain.Entities;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates;

public class Race: Aggregate<RaceId>
{
    public string Label { get; private set; }
    public Driver Winner { get; private set; } = default!;
    public List<Driver> Drivers { get; private set; } = new List<Driver>();

    public static Race Create(RaceId id, string label, List<Driver> drivers)
    {
        if (drivers.Count < 2)
        {
            throw new ArgumentException("At least two drivers are required for a race.");
        }

        var race = new Race
        {
            Id = id,
            Label = label
        };
        race.SetDrivers(drivers);
        return race;
    }
    public void SetDrivers(List<Driver> drivers)
    {
        Drivers = drivers;
    }

    public void SetWinner(Driver winner)
    {
        Winner = winner;
    }
    
}

// public void AddDriver(Driver driver)
// {
//     if (_drivers.Count > 0)
//     {
//         if (driver.CarType != _drivers[0].CarType)
//         {
//             throw new InvalidOperationException("All drivers in the race must have the same car type");
//         }
//
//         foreach (var existingDriver in _drivers)
//         {
//             if (Math.Abs(existingDriver.HorsePower - driver.HorsePower) > 100)
//             {
//                 throw new InvalidOperationException("Difference in horsepower between drivers cannot exceed 100");
//             }
//         }
//     }
//
//     _drivers.Add(driver);
// }
//
// public IEnumerable<Driver> FindDrivers(string carType, int minPower)
// {
//     if (string.IsNullOrEmpty(carType))
//     {
//         throw new ArgumentNullException(nameof(carType));
//     }
//     return _drivers.Where(driver => driver.CarType == carType && driver.HorsePower >= minPower);
// }
//
// public void SimulateRace()
// {
//     if (_drivers.Count != 2)
//     {
//         throw new InvalidOperationException("Not enough drivers to simulate race");
//     }
//     var random = new Random();
//     _winner = _drivers[random.Next(_drivers.Count)];
// }