﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.EventBus;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.Exceptions;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Features;

public record CreateDriverCommand(string name, string carType, int horsePower) : IRequest<CreateDriverResult>
{
    public Guid Id { get; init; } = Guid.NewGuid();
}

public record CreateDriverResult(Guid Id);

public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, CreateDriverResult>
{
    private readonly RaceContext _db;
    private readonly KafkaProducerService _kafkaProducerService;


    public CreateDriverCommandHandler(RaceContext db, KafkaProducerService kafkaProducerService)
    {
        _db = db;
        _kafkaProducerService = kafkaProducerService;
    }

    public async Task<CreateDriverResult> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = await _db.Drivers.FirstOrDefaultAsync(x => x.Name.Value == request.name,cancellationToken);

        if (driver is not null)
        {
            throw new DriverAlreadyExistsException("Driver already exists");
        }

        var driverEntity = _db.Drivers.Add(Driver.Create(DriverId.Of(Guid.NewGuid()), Name.Of(request.name),
            CarType.Of(request.carType), HorsePower.Of(request.horsePower))).Entity;
        await _db.SaveChangesAsync(cancellationToken);

        return new CreateDriverResult(driverEntity.Id.Value);
    }
}