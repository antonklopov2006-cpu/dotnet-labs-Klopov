namespace GeometricShapes;

// Thrown when a shape gets a zero or negative size.
public class InvalidDimensionException : Exception
{
    public InvalidDimensionException()
        : base("Shape dimension must be greater than zero.")
    {
    }

    public InvalidDimensionException(string message)
        : base(message)
    {
    }

    public InvalidDimensionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
