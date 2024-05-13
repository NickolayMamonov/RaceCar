using MediatR;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Features;

public class GetAllDrivers
{
    public record GetAllDriversQuery() : IRequest<GetAllDriversResult>;

    public record GetAllDriversResult(List<DriverDto> Drivers);

    public record DriverDto(Guid Id, string Name, string CarType, int HorsePower);

    public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, GetAllDriversResult>
    {
        private readonly RaceContext _context;

        public GetAllDriversQueryHandler(RaceContext context)
        {
            _context = context;
        }
        public async Task<GetAllDriversResult> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
        {
            var drivers = _context.Drivers.Select(d => new DriverDto(d.Id.Value, d.Name.Value, d.CarType.Value, d.HorsePower.Value)).ToList();
            return new GetAllDriversResult(drivers);
        }
    }
}
