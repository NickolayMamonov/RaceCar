using MediatR;
using Messages;
using Newtonsoft.Json;
using RaceCar.Application.EventBus;
using RaceCar.Domain.Aggregates.Events;

namespace RaceCar.Application.Handlers;

public class RaceEndedEventHandler : INotificationHandler<RaceEndedDomainEvent>
{
    private readonly KafkaProducerService _kafkaProducerService;

    public RaceEndedEventHandler(KafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
    }

    public async Task Handle(RaceEndedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = new RaceEndedMessage(notification.Id,notification.TypeOfCar, notification.EndedAt, notification.WinnerId);
        var serializedMessage = JsonConvert.SerializeObject(message);

        await _kafkaProducerService.ProduceAsync("RaceEnded", serializedMessage);

        // Console.WriteLine(
        //     $"Race ended at {notification.EndedAt}: RaceId={notification.Id}, WinnerId={notification.WinnerId}");
    }
}