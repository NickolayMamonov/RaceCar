using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates.Events;
using RaceCar.Domain.Entities;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;


namespace RaceCar.Domain.Aggregates;

public class Race : Aggregate<RaceId>
{
    public Label Label { get; private set; }
    public TypeOfCar TypeOfCar { get; private set; }
    public string? Winner { get; private set; }
    public List<Driver> Drivers { get;private set; }

    public Race()
    {
        Drivers = new List<Driver>();
    }
    public static Race Create(RaceId id, Label label, TypeOfCar typeOfCar, List<Driver> drivers)
    {
        var race = new Race()
        {
            Id = id,
            Label = label,
            TypeOfCar = typeOfCar,
            Drivers = drivers
        };
        var @event = new RaceCreatedDomainEvent(
            race.Id,
            race.Label,
            race.TypeOfCar,
            race.Drivers.Select(d => d.Id).ToList());
        race.AddDomainEvent(@event);
        
        return race;
    }

    
    public void SetWinner(string winner)
    {
        Winner = winner;
    }
}