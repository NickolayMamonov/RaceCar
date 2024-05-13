namespace RaceCar.Handlers;

using MediatR;
using RaceCar.Domain.Aggregates.Events;
using System.Threading;
using System.Threading.Tasks;

public class RaceCreatedEventHandler : INotificationHandler<RaceCreatedDomainEvent>
{
    public Task Handle(RaceCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"New race created at {DateTime.Now}: ID={notification.Id}, Label={notification.Label}, DriverIds={string.Join(", ", notification.Drivers)}");
        return Task.CompletedTask;
    }
}