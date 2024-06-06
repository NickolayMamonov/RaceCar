using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Features;

public record GetAllRacesQuery() : IRequest<IList<RaceDto>>;

public class GetAllRacesQueryHandler : IRequestHandler<GetAllRacesQuery, IList<RaceDto>>
{
    private readonly RaceContext _db;

    public GetAllRacesQueryHandler(RaceContext db)
    {
        _db = db;
    }

    public async Task<IList<RaceDto>> Handle(GetAllRacesQuery request, CancellationToken cancellationToken)
    {
        return await _db.Races
            .AsNoTrackingWithIdentityResolution()
            .Select(d => new RaceDto(d.Id.Value.ToString(), d.Label.Value, d.TypeOfCar.Value, d.DriverIds.ToList()))
            .ToListAsync();
    }
}