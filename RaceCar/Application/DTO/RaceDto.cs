using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.DTO;

public record RaceDto(string Id, string Label,string TypeOfCar);

public record RaceInputModel(string Label,string TypeOfCar);

public record SimulateRaceInputModel(Guid id);