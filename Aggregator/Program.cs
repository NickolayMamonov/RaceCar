using Consul;
using Newtonsoft.Json;
using RaceCar.Application.DTO;
using RaceCar.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConsulClient>(p => new ConsulClient(consulConfig =>
{
    consulConfig.Address = new Uri("http://localhost:8500");
}));
builder.Services.Configure<ServiceDiscoveryConfig>(builder.Configuration.GetSection("ServiceDiscoveryConfig"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseConsul();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/driver-stats", async () =>
{
    using var client = new HttpClient();
    
    try
    {
        var racesRequest = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7066/race-service/api/races");
        var racesResponse = await client.SendAsync(racesRequest);
        
        if (!racesResponse.IsSuccessStatusCode)
        {
            var racesError = await racesResponse.Content.ReadAsStringAsync();
            return Results.Problem($"Ошибка при получении гонок. Status: {racesResponse.StatusCode}. Error: {racesError}");
        }
            
        var races = JsonConvert.DeserializeObject<List<RaceDto>>(await racesResponse.Content.ReadAsStringAsync());
        
        var driversRequest = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7066/race-service/api/drivers");
        var driversResponse = await client.SendAsync(driversRequest);
        
        if (!driversResponse.IsSuccessStatusCode)
        {
            var driversError = await driversResponse.Content.ReadAsStringAsync();
            return Results.Problem($"Ошибка при получении водителей. Status: {driversResponse.StatusCode}. Error: {driversError}");
        }
            
        var drivers = JsonConvert.DeserializeObject<List<DriverDto>>(await driversResponse.Content.ReadAsStringAsync());
        
        if (drivers == null || races == null)
            return Results.NotFound("Данные не найдены");

        var stats = drivers
            .GroupBy(d => d.CarType)
            .Select(g => new
            {
                CarType = g.Key,
                DriverCount = g.Count(),
                AverageHorsePower = g.Average(d => d.HorsePower),
                Wins = races.Count(r => r.TypeOfCar == g.Key && !string.IsNullOrEmpty(r.Winner)),
                DriversInCategory = g.Select(d => new { 
                    Id = d.Id,
                    Name = d.Name,
                    HorsePower = d.HorsePower
                }).ToList()
            })
            .ToList();
        
        return Results.Ok(new
        {
            Summary = new
            {
                TotalDrivers = drivers.Count,
                TotalRaces = races.Count,
                ActiveRaces = races.Count(r => string.IsNullOrEmpty(r.Winner)),
                CompletedRaces = races.Count(r => !string.IsNullOrEmpty(r.Winner))
            },
            CarTypeStatistics = stats
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
})
.WithName("GetDriverStats")
.WithOpenApi();

app.Run();