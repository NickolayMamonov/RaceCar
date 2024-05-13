using MediatR;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Features;

public class CreateRace
{
    public record CreateRaceCommand(string Label, List<string> DriverNames) : IRequest<CreateRaceResult>
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
    public record CreateRaceResult(Guid Id);

    public class CreateRaceCommandHandler : IRequestHandler<CreateRaceCommand, CreateRaceResult>
    {
        private readonly RaceContext _context;

        public CreateRaceCommandHandler(RaceContext context)
        {
            _context = context;
        }
        public async Task<CreateRaceResult> Handle(CreateRaceCommand request, CancellationToken cancellationToken)
        {
            var drivers = _context.Drivers.Where(e => request.DriverNames.Contains(e.Name)).ToList();
            if (drivers.Count != request.DriverNames.Count)
            {
                throw new Exception("Some drivers do not exist.");
            }
            var raceEntity = _context.Races.Add(Race.Create(RaceId.Of(request.Id), Label.Of(request.Label), drivers.Select(d => d.Id).ToList()));
            await _context.SaveChangesAsync();

            return new CreateRaceResult(raceEntity.Entity.Id.Value);
        }
    }
}