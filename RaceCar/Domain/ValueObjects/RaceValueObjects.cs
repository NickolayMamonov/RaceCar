namespace RaceCar.Domain.ValueObjects;

public class Label  
{
    public string Value { get; }

    private Label(string value)
    {
        Value = value;
    }
    public static Label Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        return new Label(value);
    }

    public static implicit operator string(Label name)
    {
        return name.Value;
    }
}
public class RaceId
{
    public Guid Value { get; }

    private RaceId(Guid value)
    {
        Value = value;
    }
    public static RaceId Of(Guid value)
    {
        if (value==Guid.Empty) throw new ArgumentNullException(nameof(value));
        return new RaceId(value);
    }

    public static implicit operator Guid(RaceId raceId)
    {
        return raceId.Value;
    }
}