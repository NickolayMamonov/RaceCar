namespace RaceCar.Domain.Exceptions;


public class RaceAlreadyExistsException : Exception
{
    public RaceAlreadyExistsException(string message) : base(message)
    {
    }
   
  
}
public class InvalidRaceIdException : Exception
{
    public InvalidRaceIdException(Guid accountId): base(message: $"Race with id: {accountId} already exists.")
    {
        
    }
}