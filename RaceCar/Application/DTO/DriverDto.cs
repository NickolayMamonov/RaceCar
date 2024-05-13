using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.DTO;

// public record DriverDto(Guid Id,string Name, string CarType, int HorsePower,Guid RaceId);
public record DriverDto(Guid Id, string Name, string CarType, int HorsePower, Guid RaceId);