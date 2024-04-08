namespace RaceCar.Application.DTO;

public class DriverDto
{
    public Guid DriverId { get; set; }
    public string Name { get; set; }
    public string CarType { get; set; }
    public int HorsePower { get; set; }
}