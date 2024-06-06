using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.EventBus;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.Exceptions;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Features;

public class CreateRace
{

    public record CreateRaceCommand(string Label,string TypeOfCar) : IRequest<CreateRaceResult>
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
    public record CreateRaceResult(Guid Id);

    public class CreateRaceCommandHandler : IRequestHandler<CreateRaceCommand, CreateRaceResult>
    {
        private readonly RaceContext _db;
        private readonly KafkaProducerService _kafkaProducerService;

        public CreateRaceCommandHandler(RaceContext db,KafkaProducerService kafkaProducerService)
        {
            _db = db;
            _kafkaProducerService= kafkaProducerService;
        }
        
        
        public async Task<CreateRaceResult> Handle(CreateRaceCommand request, CancellationToken cancellationToken)
        {
            var race = await _db.Races.SingleOrDefaultAsync(x => x.Label.Value == request.Label);

            if (race is not null)
            {
                throw new RaceAlreadyExistsException("Race already exists");
            }

            var drivers = await SelectDrivers(request.TypeOfCar);

            var raceEntity = _db.Races.Add(Race.Create(RaceId.Of(Guid.NewGuid()), Label.Of(request.Label),TypeOfCar.Of(request.TypeOfCar), drivers)).Entity;
            await _db.SaveChangesAsync();

            return new CreateRaceResult(raceEntity.Id.Value);
        }

        private async Task<List<Driver>> SelectDrivers(string carType)
        {
            var allDrivers = await _db.Drivers.Where(d => d.CarType.Value == carType).ToListAsync();

            if (!allDrivers.Any())
            {
                throw new Exception($"No drivers with car type {carType} available in the database.");
            }

            var random = new Random();
            var selectedDrivers = allDrivers
                .OrderBy(d => random.Next())
                .ToList();

            return selectedDrivers;
        }
    }
}