using RaceCar.Domain.Aggregates;

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

public class TypeOfCar
{
    public string Value { get;}

    private TypeOfCar(string value)
    {
        Value = value;
    }

    public static TypeOfCar Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        return new TypeOfCar(value);
    }

    public static implicit operator string(TypeOfCar carType)
    {
        return carType.Value;
    }
}

public class Winner
{
    public string Value { get;}

    private Winner(string value)
    {
        Value = value;
    }
    public static Winner Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        return new Winner(value);
    }
    public static implicit operator string(Winner carType)
    {
        return carType.Value;
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


public class DriverIdList : ValueObject
{
    private readonly List<Driver> _driverIds;

    public DriverIdList(List<Driver> driverIds)
    {
        _driverIds = driverIds ?? throw new ArgumentNullException(nameof(driverIds));
    }

    public IReadOnlyList<Driver> Value => _driverIds.AsReadOnly();

    // Add this property
    public List<Driver> DriverIds => _driverIds;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var driverId in _driverIds)
        {
            yield return driverId;
        }
    }
}