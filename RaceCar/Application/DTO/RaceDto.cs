namespace RaceCar.Application.DTO;

public class RaceDto
{
    public string Label { get; set; }
    private List<DriverDto> Drivers { get; set; }
}