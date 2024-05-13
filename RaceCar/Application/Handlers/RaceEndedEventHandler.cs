using MediatR;
using RaceCar.Domain.Aggregates.Events;

namespace RaceCar.Handlers;

public class RaceEndedEventHandler : INotificationHandler<RaceEndedDomainEvent>
{
    public Task Handle(RaceEndedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Race ended at {notification.EndedAt}: WinnerId={notification.WinnerId}");
        return Task.CompletedTask;
    }
}