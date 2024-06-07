using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.EventBus;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.Exceptions;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Features;

public record CreateRaceCommand(string Label, string TypeOfCar) : IRequest<CreateRaceResult>
{
    public Guid Id { get; init; } = Guid.NewGuid();
}

public record CreateRaceResult(Guid Id);

public class CreateRaceCommandHandler : IRequestHandler<CreateRaceCommand, CreateRaceResult>
{
    private readonly RaceContext _db;
    private readonly KafkaProducerService _kafkaProducerService;

    public CreateRaceCommandHandler(RaceContext db, KafkaProducerService kafkaProducerService)
    {
        _db = db;
        _kafkaProducerService = kafkaProducerService;
    }


    public async Task<CreateRaceResult> Handle(CreateRaceCommand request, CancellationToken cancellationToken)
    {
        var race = await _db.Races.FirstOrDefaultAsync(x => x.Label.Value == request.Label,cancellationToken);

        if (race is not null)
        {
            throw new RaceAlreadyExistsException("Race already exists");
        }

        var driversWithSameCarType = await _db.Drivers
            .Where(d => d.CarType.Value == request.TypeOfCar && d.HorsePower.Value != null)
            .ToListAsync(cancellationToken);

        driversWithSameCarType.Sort((d1, d2) => d1.HorsePower.Value.CompareTo(d2.HorsePower.Value));

        var selectedDrivers = new List<Driver>();

        for (int i = 0; i < driversWithSameCarType.Count - 1; i++)
        {
            selectedDrivers.Add(driversWithSameCarType[i]);
            selectedDrivers.Add(driversWithSameCarType[i + 1]);
            break;
        }

        var raceEntity = Race.Create(RaceId.Of(Guid.NewGuid()), Label.Of(request.Label),TypeOfCar.Of(request.TypeOfCar), selectedDrivers);

        // Save the race first before adding drivers
        _db.Races.Add(raceEntity);
        await _db.SaveChangesAsync(cancellationToken);

        if (selectedDrivers.Count == 0)
        {
            throw new Exception("No drivers in the race");
        }

        Random random = new Random();
        var winner = selectedDrivers[random.Next(selectedDrivers.Count)];

        // Set the winner
        raceEntity.SetWinner(winner.Name.Value);

        _db.Races.Update(raceEntity);
        await _db.SaveChangesAsync(cancellationToken);

        return new CreateRaceResult(raceEntity.Id.Value);
    }
}