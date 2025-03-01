﻿using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.DTO;

// public record DriverDto(Guid Id,string Name, string CarType, int HorsePower,Guid RaceId);
public record DriverDto(string Id, string Name, string CarType, int HorsePower);

public record DriverInputModel(string Name, string CarType, int HorsePower);