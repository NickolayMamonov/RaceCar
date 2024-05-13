using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.DTO;

public record RaceDto(Guid Id, string? Label, List<DriverId> DriverIds);