using MediatR;
using RaceCar.Application.DTO;
using RaceCar.Application.Services;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.Exceptions;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Features;

public class CreateDriver
{
    public record CreateDriverCommand(Name name, CarType carType, HorsePower horsePower) : IRequest<CreateDriverResult>, IRequest<DriverDto>
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }

    public record CreateDriverResult(Guid Id);

    public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, CreateDriverResult>
    {
        private readonly IDriverService _driverService;

        public CreateDriverCommandHandler(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public async Task<CreateDriverResult> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var driverDto = await _driverService.AddDriverAsync(request.name, request.carType, request.horsePower);
            return new CreateDriverResult(driverDto.Id);
        }
    }
}