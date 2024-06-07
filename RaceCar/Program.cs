using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Application.EventBus;
using RaceCar.Application.Features;
using RaceCar.Application.Services;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.Aggregates.Events;
using RaceCar.Domain.ValueObjects;
using RaceCar.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RaceContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=racecar;Username=postgres;Password=postgres");
});


// builder.Services.AddScoped<IDriverService, DriverService>();
// builder.Services.AddScoped<IRaceService, RaceService>();

builder.Services.AddSingleton<KafkaProducerService>();

// builder.Services.AddScoped<INotificationHandler<RaceCreatedDomainEvent>, RaceCreatedEventHandler>();
// builder.Services.AddScoped<INotificationHandler<RaceDriversFilledDomainEvent>, RaceDriversFilledEventHandler>();
// builder.Services.AddScoped<INotificationHandler<RaceEndedDomainEvent>, RaceEndedEventHandler>();
// builder.Services.AddScoped<INotificationHandler<DriverCreatedDomainEvent>, DriverCreatedDomainEventHandler>();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

SeedDataDrivers(app);
SeedDataRaces(app);

void SeedDataRaces(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope() )
    {
        using (var db = scope.ServiceProvider.GetRequiredService<RaceContext>())
        {
            db.Database.EnsureCreated();
            if (!db.Races.Any())
            {
                var drivers = db.Drivers.Take(2).ToList(); // Get the first two drivers from the database
                if(drivers.Count < 2)
                {
                    throw new Exception("At least two drivers are required to create a race.");
                }
                db.Races.Add(Race.Create(RaceId.Of(Guid.NewGuid()), Label.Of("Formula"),TypeOfCar.Of("Sedan"), drivers));
                db.SaveChanges();
            }
        }
    }
}

// Seed data
void SeedDataDrivers(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        using (var db = scope.ServiceProvider.GetRequiredService<RaceContext>())
        {
            db.Database.EnsureCreated();
            if (db.Drivers.Count() < 2)
            {
                db.Drivers.Add(Driver.Create(DriverId.Of(Guid.NewGuid()), Name.Of("Nico Rossberg"), CarType.Of("Sedan"), HorsePower.Of(300)));
                db.Drivers.Add(Driver.Create(DriverId.Of(Guid.NewGuid()), Name.Of("Bob Rossberg"), CarType.Of("Sedan"), HorsePower.Of(320)));

                db.SaveChanges();
            }
        }
    }
}



app.MapPost("api/drivers", async (DriverInputModel model, IMediator mediator) =>
{
    var command = new CreateDriverCommand(model.Name, model.CarType, model.HorsePower);
    var response = await mediator.Send(command);
    return Results.Created($"/api/drivers/{response.Id}", response);
});

app.MapGet("api/drivers", async (RaceContext db,IMediator mediator) =>
{
    return await mediator.Send(new GetAllDriversQuery());
});

app.MapPost("api/races", async (RaceInputModel model, IMediator mediator,RaceContext db) =>
{
    var command = new CreateRaceCommand(model.Label, model.TypeOfCar);
    var response = await mediator.Send(command);
    return Results.Created($"/api/races/{response.Id}", response);
});
app.MapGet("api/races", async (RaceContext db,IMediator mediator) =>
{
    return await mediator.Send(new GetAllRacesQuery());
});



app.MapPost("api/races/simulate", async (SimulateRaceInputModel model, IMediator mediator) =>
{
    var command = new SimulateRaceCommand(model.id);
    var race = await mediator.Send(command);
    return Results.Ok(race);
});


app.Run();


