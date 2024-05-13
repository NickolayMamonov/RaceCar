namespace RaceCar.Domain.Exceptions;

public class DriverAlreadyExistsException : Exception
{
    public DriverAlreadyExistsException(string message) : base(message)
    {
    }
}