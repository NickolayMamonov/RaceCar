using Consul;
using Microsoft.Extensions.Options;
using IApplicationLifetime = Microsoft.Extensions.Hosting.IApplicationLifetime;

namespace RaceCar.Infrastructure;

public static class ConsulBuilderExtensions
{
    public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
    {
        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
        
        var settings = app.ApplicationServices.GetRequiredService<IOptions<ServiceDiscoveryConfig>>();
        var serviceName = settings.Value.Name;
        var serviceId = settings.Value.Id;
        var uri = new Uri($"http://{settings.Value.Host}:{settings.Value.Port}");
        var registration = new AgentServiceRegistration()
        {
            ID = serviceId,
            Name = serviceName,
            Address = $"{settings.Value.Host}",
            Port = uri.Port,
            Tags = new[] { $"urlprefix-/{settings.Value.Id}" }
        };
        var result = consulClient.Agent.ServiceDeregister(registration.ID).Result;
        result = consulClient.Agent.ServiceRegister(registration).Result;
        lifetime.ApplicationStopping.Register(() =>
        {
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });
        return app;
    }
}

public class ServiceDiscoveryConfig
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Host { get; init; }
    public int Port { get; init; }
}