using MediatR;
using RaceCar.Domain.Aggregates.Events;

namespace RaceCar.Handlers;

public class RaceDriversFilledEventHandler : INotificationHandler<RaceDriversFilledDomainEvent>
{
    public Task Handle(RaceDriversFilledDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Race filled with drivers at {notification.FilledAt}: DriverIds={string.Join(", ", notification.Drivers)}");
        return Task.CompletedTask;
    }
}