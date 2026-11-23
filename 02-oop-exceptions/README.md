# Practical work 2. OOP and exception handling

Variant 1 - Geometric shapes.

- Abstract `Shape` -> `Circle`, `Rectangle` (with an overloaded constructor for a square)
- `IComparable<Shape>` compares shapes by area, so `List<Shape>.Sort()` works
- Custom `InvalidDimensionException` for zero or negative sizes
- Menu: add a shape, show all, sort by area, totals, filter by type

Run: open `NET_PR2_Klopov.sln` in Visual Studio. Requires .NET 10 SDK.

Author: Klopov A.K., group 3PI-24B
