namespace GeometricShapes;

public class Rectangle : Shape
{
    private double _width;
    private double _height;

    public Rectangle(string name, double width, double height)
        : base(name)
    {
        Width = width;
        Height = height;
    }

    // Overloaded constructor for a square.
    public Rectangle(string name, double side)
        : this(name, side, side)
    {
    }

    public double Width
    {
        get => _width;
        set => _width = ValidateDimension(value, "Width");
    }

    public double Height
    {
        get => _height;
        set => _height = ValidateDimension(value, "Height");
    }

    public override double GetArea() => Width * Height;

    public override double GetPerimeter() => 2 * (Width + Height);

    public override string GetDescription() => $"Rectangle, {Width:F2} x {Height:F2}";
}
