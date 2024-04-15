using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.DTO;

public class DriverDto
{
    public Guid DriverId { get; set; }
    public Name Name { get; set; }
    public CarType CarType { get; set; }
    public HorsePower HorsePower { get; set; }
}