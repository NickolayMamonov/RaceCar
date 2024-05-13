using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Features;

public class GetAllDrivers
{
    public record GetAllDriversQuery() : IRequest<GetAllDriversResult>;

    public record GetAllDriversResult(List<DriverDto> Drivers);


    public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, GetAllDriversResult>
    {
        private readonly RaceContext _context;

        public GetAllDriversQueryHandler(RaceContext context)
        {
            _context = context;
        }

        public async Task<GetAllDriversResult> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _context.Drivers.ToListAsync(cancellationToken);

            return new GetAllDrivers.GetAllDriversResult(
                drivers.Select(d =>
                        new DriverDto(d.Id, d.Name.Value, d.CarType.Value, d.HorsePower.Value, d.RaceId ?? Guid.Empty))
                    .ToList()
            );
        }
    }
}