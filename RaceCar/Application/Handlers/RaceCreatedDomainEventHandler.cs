using MediatR;
using Messages;
using Newtonsoft.Json;
using RaceCar.Application.EventBus;
using RaceCar.Domain.Aggregates.Events;

namespace RaceCar.Application.Handlers;

public class RaceCreatedDomainEventHandler: INotificationHandler<RaceCreatedDomainEvent>
{
    private readonly KafkaProducerService _kafkaProducerService;
    

    public RaceCreatedDomainEventHandler(KafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
    }


    public async Task Handle(RaceCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = new RaceCreatedMessage(notification.Id, notification.Label, notification.TypeOfCar,
            notification.Winner,new DateTimeOffset(DateTime.Now.ToUniversalTime()).ToUnixTimeSeconds().ToString()
        );
        var json = JsonConvert.SerializeObject(message);
        await _kafkaProducerService.ProduceAsync("RaceCreated", json);
    }
}