using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.DTO;

public record DriverDto(string Id, string Name, string CarType, int HorsePower);
// public class DriverDto
// {
//     public Guid DriverId { get; set; }
//     public Name Name { get; set; }
//     public CarType CarType { get; set; }
//     public HorsePower HorsePower { get; set; }
// }