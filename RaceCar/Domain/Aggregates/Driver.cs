using RaceCar.Domain.Aggregates.Events;
using RaceCar.Domain.Entities;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates;

public class Driver: Aggregate<DriverId>
{
    public Name Name { get; private set; }
    public CarType CarType { get; private set; }
    public HorsePower HorsePower { get; private set; }
    public Guid? RaceId { get; private set; }
    
    public void AssignToRace(Guid raceId)
    {
        RaceId = raceId;
    }
    
    public static Driver Create(DriverId id,Name name, CarType carType, HorsePower horsePower)
    {
        var driver = new Driver
        {
            Id = id,
            Name = name,
            CarType = carType,
            HorsePower = horsePower
        };

        // var @event = new DriverCreatedDomainEvent(
        //     driver.Id,
        //     driver.Name,
        //     driver.CarType,
        //     driver.HorsePower);

        // driver.AddDomainEvent(@event);
        return driver;
    }
    // public void SetRaceId(RaceId? raceId)
    // {
    //     RaceId = raceId;
    // }

}