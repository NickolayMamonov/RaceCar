namespace Messages;

public record RaceCreatedMessage(Guid Id,string Label,string TypeOfCar,string Winner, string Timestamp);