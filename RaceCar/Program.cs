using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Application.Services;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.Aggregates.Events;
using RaceCar.Domain.ValueObjects;
using RaceCar.Features;
using RaceCar.Handlers;
using RaceCar.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RaceContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=Race;Username=postgres;Password=root",
        b => b.MigrationsAssembly("Race"));
});
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IRaceService, RaceService>();

builder.Services.AddScoped<INotificationHandler<RaceCreatedDomainEvent>, RaceCreatedEventHandler>();
builder.Services.AddScoped<INotificationHandler<RaceDriversFilledDomainEvent>, RaceDriversFilledEventHandler>();
builder.Services.AddScoped<INotificationHandler<RaceEndedDomainEvent>, RaceEndedEventHandler>();
builder.Services.AddScoped<INotificationHandler<DriverCreatedDomainEvent>, DriverCreatedDomainEventHandler>();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapPost("/api/driver", async (IMediator mediator, Name name, CarType carType, HorsePower horsePower) =>
{
    try
    {
        var result = await mediator.Send(new CreateDriver.CreateDriverCommand(name, carType, horsePower));
        return Results.Created($"/api/driver/", result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/drivers", async (IMediator mediator) =>
{
    try
    {
        var drivers = await mediator.Send(new GetAllDrivers.GetAllDriversQuery());
        return Results.Ok(drivers);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/api/race", async (IMediator mediator, Label raceName) =>
{
    try
    {
        var command = new CreateRace.CreateRaceCommand(raceName);
        var race = await mediator.Send(command);
        return Results.Created($"/api/race/{race.Id}", race);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapGet("/api/races", async (IMediator mediator) =>
{
    try
    {
        var query = new GetAllRaces.GetAllRacesQuery();
        var result = await mediator.Send(query);
        return Results.Ok(result.Races);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPost("/api/race/simulate", async (IMediator mediator, RaceId raceId) =>
{
    try
    {
        var command = new SimulateRace.SimulateRaceCommand(raceId);
        var race = await mediator.Send(command);
        return Results.Ok(race);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.Run();


