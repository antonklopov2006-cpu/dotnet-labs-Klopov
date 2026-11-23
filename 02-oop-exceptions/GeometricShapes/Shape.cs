namespace GeometricShapes;

public abstract class Shape : IComparable<Shape>
{
    private string _name = string.Empty;

    protected Shape(string name)
    {
        Name = name;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Shape name cannot be empty.", nameof(Name));
            }

            _name = value.Trim();
        }
    }

    public abstract double GetArea();

    public abstract double GetPerimeter();

    // Type and sizes of the shape, e.g. "Circle, r = 2.00".
    public virtual string GetDescription() => GetType().Name;

    public int CompareTo(Shape? other)
    {
        if (other is null)
        {
            return 1;
        }

        return GetArea().CompareTo(other.GetArea());
    }

    public override string ToString() =>
        $"{Name,-14} {GetDescription(),-28} area = {GetArea(),9:F2}   perimeter = {GetPerimeter(),8:F2}";

    // Shared check for all sizes of derived classes.
    protected static double ValidateDimension(double value, string dimensionName)
    {
        if (value <= 0)
        {
            throw new InvalidDimensionException(
                $"{dimensionName} must be greater than zero, but was {value}.");
        }

        return value;
    }
}
