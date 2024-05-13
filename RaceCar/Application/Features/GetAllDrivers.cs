using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Application.Services;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Features;

public class GetAllDrivers
{
    public record GetAllDriversQuery() : IRequest<GetAllDriversResult>, IRequest<List<DriverDto>>;

    public record GetAllDriversResult(List<DriverDto> Drivers);


    public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, List<DriverDto>>
    {
        private readonly IDriverService _driverService;

        public GetAllDriversQueryHandler(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public async Task<List<DriverDto>> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
        {
            return await _driverService.GetAllDriversAsync();
        }
    }
}