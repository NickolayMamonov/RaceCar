namespace Messages;

public record DriverCreatedMessage(Guid Id,string Name,string CarType, int HorsePower, string Timestamp);