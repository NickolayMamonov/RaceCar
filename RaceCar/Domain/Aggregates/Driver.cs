using RaceCar.Domain.Entities;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates;

public class Driver: Aggregate<DriverId>
{
    public Name Name { get; private set; }
    public CarType CarType { get; private set; }
    public HorsePower HorsePower { get; private set; }
    public RaceId RaceId { get; private set; }
    
    public Driver(DriverId id,Name name, CarType carType, HorsePower horsePower)
    {
        Id = id;
        Name = name;
        CarType = carType;
        HorsePower = horsePower;
       
    }
    public void SetRaceId(RaceId raceId)
    {
        RaceId = raceId;
    }

}