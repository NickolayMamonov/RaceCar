using MediatR;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Features;

public class CreateDriver
{
    public record CreateDriverCommand(string Name, string CarType, int HorsePower) : IRequest<CreateDriverResult>
    {
        public Guid Id { get; init; }= Guid.NewGuid();
    }
    public record CreateDriverResult(Guid Id);

    public class CreateDriverCommandHandler:IRequestHandler<CreateDriverCommand,CreateDriverResult>
    {
        private readonly RaceContext _context;

        public CreateDriverCommandHandler(RaceContext context)
        {
            _context = context;
        }
        public async Task<CreateDriverResult> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = _context.Drivers.FirstOrDefault(e => e.Name == request.Name);
            if (driver is not null)
            {
                throw new ArgumentNullException();
            }
            var driverEntity = _context.Drivers.Add(Driver.Create(DriverId.Of(Guid.NewGuid()), Name.Of(request.Name),
                CarType.Of(request.CarType), HorsePower.Of(request.HorsePower)));
            await _context.SaveChangesAsync();
            return new CreateDriverResult(driverEntity.Entity.Id.Value);
        }
    }
}