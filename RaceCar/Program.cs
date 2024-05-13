using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Application.Services;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.Aggregates.Events;
using RaceCar.Domain.ValueObjects;
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

app.MapPost("/api/driver", async (IDriverService driverService, Name name, CarType carType, HorsePower horsePower) =>
{
    try
    {
        var driver = await driverService.AddDriverAsync(name, carType, horsePower);
        return Results.Created($"/api/driver/{driver.Id}", driver);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/api/race", async (IRaceService raceService, Label raceName) =>
{
    try
    {
        var race = await raceService.CreateRaceAsync(raceName);
        return Results.Created($"/api/race/{race.Id}", race);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPost("/api/race/simulate", async (IRaceService raceService, RaceId raceId) =>
{
    try
    {
        var winner = await raceService.SimulateRace(raceId);
        return Results.Ok(winner);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();


