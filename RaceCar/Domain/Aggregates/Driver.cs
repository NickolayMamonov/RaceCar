using RaceCar.Domain.Entities;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Domain.Aggregates;

public class Driver: Aggregate<DriverId>
{
    public string Name { get; private set; }
    public string CarType { get; private set; }
    public int HorsePower { get; private set; }
    public Guid? RaceId { get; private set; }
    
    private Driver(){}

    public Driver(DriverId id,string name, string carType, int horsePower )
    {
        Id = id;
        Name = name;
        CarType = carType;
        HorsePower = horsePower;
    }
    public void SetRaceId(Guid raceId)
    {
        RaceId = raceId;
    }

}