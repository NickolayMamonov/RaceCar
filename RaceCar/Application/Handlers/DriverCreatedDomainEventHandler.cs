using MediatR;
using Messages;
using Newtonsoft.Json;
using RaceCar.Application.EventBus;
using RaceCar.Domain.Aggregates.Events;

namespace RaceCar.Application.Handlers;

public class DriverCreatedDomainEventHandler : INotificationHandler<DriverCreatedDomainEvent>
{
    private readonly KafkaProducerService _kafkaProducerService;
    

    public DriverCreatedDomainEventHandler(KafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
    }

    public async Task Handle(DriverCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = new DriverCreatedMessage(notification.Id, notification.Name, notification.CarType,
            notification.HorsePower,new DateTimeOffset(DateTime.Now.ToUniversalTime()).ToUnixTimeSeconds().ToString()
        );
        var json = JsonConvert.SerializeObject(message);
        await _kafkaProducerService.ProduceAsync("DriverCreated", json);
        // Console.WriteLine(
        //     $"New driver created {DateTime.Now}: ID={notification.Id}, Name={notification.Name}, CarType={notification.CarType}, HorsePower={notification.HorsePower}");
        // return Task.CompletedTask;
    }
}