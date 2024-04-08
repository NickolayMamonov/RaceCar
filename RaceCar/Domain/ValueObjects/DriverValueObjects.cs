namespace RaceCar.Domain.ValueObjects;

public class Name
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }
    public static Name Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        return new Name(value);
    }

    public static implicit operator string(Name name)
    {
        return name.Value;
    }
}

public class CarType
{
    public string Value { get; }

    private CarType(string value)
    {
        Value = value;
    }

    public static CarType Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        return new CarType(value);
    }

    public static implicit operator string(CarType carType)
    {
        return carType.Value;
    }
}

public class HorsePower
{
    public int Value { get; }

    private HorsePower(int value)
    {
        Value = value;
    }

    public static HorsePower Of(int value)
    {
        if (value is < 50 or > 2000) throw new ArgumentException("The value of HP should be from 50 to 2000");
        return new HorsePower(value);
    }

    public static implicit operator int(HorsePower horsePower)
    {
        return horsePower.Value;
    }
}
public class DriverId
{
    public Guid Value { get; }

    private DriverId(Guid value)
    {
        Value = value;
    }
    public static DriverId Of(Guid value)
    {
        if (value==Guid.Empty) throw new ArgumentNullException(nameof(value));
        return new DriverId(value);
    }

    public static implicit operator Guid(DriverId driverId)
    {
        return driverId.Value;
    }
}