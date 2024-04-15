using RaceCar.Domain.ValueObjects;

namespace RaceCar.Application.DTO;

public class RaceDto
{
    public Label Label { get; set; }
    private List<DriverDto> Drivers { get; set; }
}