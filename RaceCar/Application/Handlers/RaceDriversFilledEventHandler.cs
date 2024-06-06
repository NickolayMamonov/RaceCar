namespace RaceCar.Application.Handlers;

// public class RaceDriversFilledEventHandler : INotificationHandler<RaceDriversFilledDomainEvent>
// {
//     private readonly KafkaProducerService _kafkaProducerService;
//
//     public RaceDriversFilledEventHandler(KafkaProducerService kafkaProducerService)
//     {
//         _kafkaProducerService = kafkaProducerService;
//     }
//
//     public async Task Handle(RaceDriversFilledDomainEvent notification, CancellationToken cancellationToken)
//     {
//         var driverIds = notification.Drivers.Select(driver => driver.Value).ToList();
//         var message = new RaceDriversFilledMessage(driverIds, notification.FilledAt);
//         var serializedMessage = JsonSerializer.Serialize(message);
//
//         await _kafkaProducerService.ProduceAsync("race-created", serializedMessage);
//
//         Console.WriteLine($"Drivers filled for race at {notification.FilledAt}");
//     }
// }