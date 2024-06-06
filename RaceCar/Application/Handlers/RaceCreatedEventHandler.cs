using MediatR;
using Messages;
using Newtonsoft.Json;
using RaceCar.Application.EventBus;
using RaceCar.Domain.Aggregates.Events;

namespace RaceCar.Application.Handlers;

public class RaceCreatedEventHandler : INotificationHandler<RaceCreatedDomainEvent>
{
    private readonly KafkaProducerService _kafkaProducerService;

    public RaceCreatedEventHandler(KafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
    }

    public async Task Handle(RaceCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        
        var message = new RaceCreatedMessage(notification.Id, notification.Label,notification.TypeOfCar,new DateTimeOffset(DateTime.Now.ToUniversalTime()).ToUnixTimeSeconds().ToString());
        var serializedMessage = JsonConvert.SerializeObject(message);

        await _kafkaProducerService.ProduceAsync("RaceCreated", serializedMessage);

        // Console.WriteLine($"Race created at {notification.CreatedAt}: RaceId={notification.Id}");
    }
}