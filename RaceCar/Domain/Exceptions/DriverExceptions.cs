namespace RaceCar.Domain.Exceptions;

public class DriverAlreadyExistsException : Exception
{
    public DriverAlreadyExistsException(string message) : base(message)
    {
    }
   
  
}

public class InvalidDriverIdException : Exception
{
    public InvalidDriverIdException(Guid accountId): base(message: $"Driver with id: {accountId} already exists.")
    {
        
    }
}