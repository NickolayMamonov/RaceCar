using RaceCar.Domain.Entities;
using RaceCar.Domain.ValueObjects;


namespace RaceCar.Domain.Aggregates;

public class Race: Aggregate<RaceId>
{
    public Label Label { get; private set; }
    public Driver Winner { get; private set; } = default!;
    public virtual ICollection<DriverId> DriverIds { get; set; }
    public static Race Create(RaceId id, Label label, List<DriverId> drivers)
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
    public void SetDrivers(List<DriverId> drivers)
    {
        DriverIds = drivers;
    }

    public void SetWinner(Driver winner)
    {
        Winner = winner;
    } 
    
}
