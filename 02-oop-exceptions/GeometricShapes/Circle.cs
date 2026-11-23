namespace GeometricShapes;

public class Circle : Shape
{
    private double _radius;

    public Circle(string name, double radius)
        : base(name)
    {
        Radius = radius;
    }

    public double Radius
    {
        get => _radius;
        set => _radius = ValidateDimension(value, "Radius");
    }

    public override double GetArea() => Math.PI * Radius * Radius;

    public override double GetPerimeter() => 2 * Math.PI * Radius;

    public override string GetDescription() => $"Circle, r = {Radius:F2}";
}
