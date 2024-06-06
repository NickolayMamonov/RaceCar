using RaceCar.Application.DTO;
using RaceCar.Domain.Entities;
using RaceCar.Domain.ValueObjects;


namespace RaceCar.Domain.Aggregates;

public class Race : Aggregate<RaceId>
{
    public Label Label { get; private set; }
    public TypeOfCar TypeOfCar { get; private set; }
    public Driver? Winner { get; private set; }
    public virtual ICollection<Driver> DriverIds { get; set; }

    public static Race Create(RaceId id, Label label, TypeOfCar typeOfCar, List<Driver> drivers)
    {
        if (drivers.Count < 2)
        {
            throw new ArgumentException("At least two drivers are required for a race.");
        }

        var race = new Race
        {
            Id = id,
            Label = label,
            TypeOfCar = typeOfCar
        };
        race.SetDrivers(drivers);
        return race;
    }

    public void SetDrivers(List<Driver> drivers)
    {
        DriverIds = drivers;
    }

    public void SetWinner(Driver winner)
    {
        Winner = winner;
    }
}