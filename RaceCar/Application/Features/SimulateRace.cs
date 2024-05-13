using MediatR;
using RaceCar.Application.Services;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Features;

public class SimulateRace
{
    public record SimulateRaceCommand(RaceId RaceId) : IRequest<Race>;

    public class SimulateRaceCommandHandler : IRequestHandler<SimulateRaceCommand, Race>
    {
        private readonly IRaceService _raceService;

        public SimulateRaceCommandHandler(IRaceService raceService)
        {
            _raceService = raceService;
        }

        public async Task<Race> Handle(SimulateRaceCommand request, CancellationToken cancellationToken)
        {
            return await _raceService.SimulateRace(request.RaceId);
        }
    }
}