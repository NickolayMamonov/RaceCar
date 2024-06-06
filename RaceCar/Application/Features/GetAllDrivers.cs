using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Features;

public record GetAllDriversQuery() : IRequest<IList<DriverDto>>;

public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, IList<DriverDto>>
{
    private readonly RaceContext _db;


    public GetAllDriversQueryHandler(RaceContext db)
    {
        _db = db;
    }


    public async Task<IList<DriverDto>> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
    {
        var drivers = await _db.Drivers.AsNoTrackingWithIdentityResolution().ToListAsync();
        return await _db.Drivers.Select(d => new DriverDto(d.Id.Value.ToString(), d.Name.Value, d.CarType.Value,
            d.HorsePower.Value, d.RaceId ?? Guid.Empty)).ToListAsync();
    }
}