using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Application.Services;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.ValueObjects;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");
// Define your endpoints
// app.MapPost("/race", async (IRaceService raceService, CreateRaceRequest request) =>
// {
//     var drivers = request.Drivers.Select(driver =>
//         new Driver(DriverId.Of(Guid.NewGuid()), driver.Name, driver.CarType, driver.HorsePower)).ToList();
//
//     //  var race = await raceService.CreateRace(request.RaceName, drivers);
//
//     //  return Results.Created($"/race/{race.Id}", race);
// });
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
        var race = await raceService.CreateRace(raceName);
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

public record CreateRaceRequest(string RaceName, List<DriverDto> Drivers);
