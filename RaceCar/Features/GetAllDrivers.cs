using MediatR;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Features;

public record GetAllDrivers
{
    public Guid Id { get; init; } = Guid.NewGuid();
}

