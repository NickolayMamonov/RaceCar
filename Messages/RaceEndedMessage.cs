namespace Messages;

public record RaceEndedMessage(Guid RaceId,string TypeOfCar, DateTime EndedAt, Guid WinnerId);