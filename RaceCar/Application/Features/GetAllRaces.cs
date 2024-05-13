using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Application.Services;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Features;

public class GetAllRaces
{
    public record GetAllRacesQuery() : IRequest<GetAllRacesResult>;

    public record GetAllRacesResult(List<RaceDto> Races);

    public class GetAllRacesQueryHandler : IRequestHandler<GetAllRacesQuery, GetAllRacesResult>
    {
        private readonly IRaceService _raceService;

        public GetAllRacesQueryHandler(IRaceService raceService)
        {
            _raceService = raceService;
        }

        public async Task<GetAllRacesResult> Handle(GetAllRacesQuery request, CancellationToken cancellationToken)
        {
            var races = await _raceService.GetAllRacesAsync();
            return new GetAllRacesResult(races);
        }
    }
}