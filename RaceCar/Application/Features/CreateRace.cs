using MediatR;
using RaceCar.Application.DTO;
using RaceCar.Application.Services;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Features;

public class CreateRace
{
    public record CreateRaceCommand(Label RaceName) : IRequest<RaceDto>;

    public class CreateRaceCommandHandler : IRequestHandler<CreateRaceCommand, RaceDto>
    {
        private readonly IRaceService _raceService;

        public CreateRaceCommandHandler(IRaceService raceService)
        {
            _raceService = raceService;
        }

        public async Task<RaceDto> Handle(CreateRaceCommand request, CancellationToken cancellationToken)
        {
            return await _raceService.CreateRaceAsync(request.RaceName);
        }
    }
}