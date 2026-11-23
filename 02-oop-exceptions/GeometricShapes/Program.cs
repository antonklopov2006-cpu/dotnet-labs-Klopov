using System.Globalization;

namespace GeometricShapes;

internal static class Program
{
    private static void Main()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        List<Shape> shapes =
        [
            new Circle("Circle A", 3),
            new Rectangle("Rect B", 4, 2.5),
            new Rectangle("Square C", 5),
            new Circle("Circle D", 1.5),
        ];

        bool isRunning = true;
        while (isRunning)
        {
            PrintMenu();
            int choice = ReadInt("Your choice: ", 0, 5);
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    AddShape(shapes);
                    break;
                case 2:
                    ShowShapes(shapes, "--- All shapes ---");
                    break;
                case 3:
                    shapes.Sort();
                    ShowShapes(shapes, "--- Shapes sorted by area ---");
                    break;
                case 4:
                    ShowTotals(shapes);
                    break;
                case 5:
                    ShowByType(shapes);
                    break;
                case 0:
                    isRunning = false;
                    Console.WriteLine("Goodbye!");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("===== Geometric shapes =====");
        Console.WriteLine("1. Add a shape");
        Console.WriteLine("2. Show all shapes");
        Console.WriteLine("3. Sort by area");
        Console.WriteLine("4. Show total area and perimeter");
        Console.WriteLine("5. Filter by type");
        Console.WriteLine("0. Exit");
    }

    private static void AddShape(List<Shape> shapes)
    {
        Console.WriteLine("Shape type: 1 - circle, 2 - rectangle, 3 - square");
        int type = ReadInt("Type: ", 1, 3);
        Console.Write("Name: ");
        string name = Console.ReadLine() ?? string.Empty;

        try
        {
            Shape shape = type switch
            {
                1 => new Circle(name, ReadDouble("Radius: ")),
                2 => new Rectangle(name, ReadDouble("Width: "), ReadDouble("Height: ")),
                _ => new Rectangle(name, ReadDouble("Side: ")),
            };

            shapes.Add(shape);
            Console.WriteLine($"Added: {shape.Name}, area = {shape.GetArea():F2}");
        }
        catch (InvalidDimensionException ex)
        {
            Console.WriteLine($"Invalid size: {ex.Message} Shape was not added.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Invalid data: {ex.Message} Shape was not added.");
        }
        finally
        {
            Console.WriteLine($"Shapes in the list: {shapes.Count}");
        }
    }

    private static void ShowShapes(List<Shape> shapes, string title)
    {
        if (shapes.Count == 0)
        {
            Console.WriteLine("The list is empty.");
            return;
        }

        Console.WriteLine(title);
        int number = 1;
        foreach (Shape shape in shapes)
        {
            // GetArea, GetPerimeter and GetDescription are called polymorphically.
            Console.WriteLine($"{number,2}. {shape}");
            number++;
        }
    }

    private static void ShowTotals(List<Shape> shapes)
    {
        if (shapes.Count == 0)
        {
            Console.WriteLine("The list is empty.");
            return;
        }

        double totalArea = 0;
        double totalPerimeter = 0;
        Shape largest = shapes[0];

        foreach (Shape shape in shapes)
        {
            totalArea += shape.GetArea();
            totalPerimeter += shape.GetPerimeter();
            if (shape.CompareTo(largest) > 0)
            {
                largest = shape;
            }
        }

        Console.WriteLine($"Shapes:          {shapes.Count}");
        Console.WriteLine($"Total area:      {totalArea:F2}");
        Console.WriteLine($"Total perimeter: {totalPerimeter:F2}");
        Console.WriteLine($"Largest shape:   {largest.Name} (area {largest.GetArea():F2})");
    }

    private static void ShowByType(List<Shape> shapes)
    {
        int type = ReadInt("Show: 1 - circles, 2 - rectangles: ", 1, 2);
        Console.WriteLine();

        int found = 0;
        foreach (Shape shape in shapes)
        {
            bool matches = type == 1 ? shape is Circle : shape is Rectangle;
            if (matches)
            {
                Console.WriteLine($"   {shape}");
                found++;
            }
        }

        if (found == 0)
        {
            Console.WriteLine("No shapes of this type.");
        }
    }

    private static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Invalid input. Enter an integer from {min} to {max}.");
        }
    }

    // Checks only that the input is a number. Zero and negative values
    // are passed on so that the shape itself throws InvalidDimensionException.
    private static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? string.Empty).Replace(',', '.');
            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            Console.WriteLine("Invalid input. Enter a number, e.g. 2.5");
        }
    }
}
