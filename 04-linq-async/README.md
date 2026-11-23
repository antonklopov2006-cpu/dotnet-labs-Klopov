# Practical work 4. LINQ to Objects and async programming

Variant 1 - Online shop price comparison.

- Three async sources (`Task.Delay` 1 / 2 / 1.5 s), parallel (`Task.WhenAll`) and sequential loading with `Stopwatch`
- LINQ: cheapest offer per product, category statistics, price range search, wish list (`join ... into`), products in all shops
- Report saved to JSON with `File.WriteAllTextAsync`

Run: open `PriceComparison.sln` in Visual Studio. Requires .NET 10 SDK.
