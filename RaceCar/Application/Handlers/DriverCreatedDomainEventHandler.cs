using MediatR;
using RaceCar.Domain.Aggregates.Events;

namespace RaceCar.Handlers;

public class DriverCreatedDomainEventHandler : INotificationHandler<DriverCreatedDomainEvent>
{
    public Task Handle(DriverCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"New driver created {DateTime.Now}: ID={notification.Id}, Name={notification.Name}, CarType={notification.CarType}, HorsePower={notification.HorsePower}");
        return Task.CompletedTask;
    }
}