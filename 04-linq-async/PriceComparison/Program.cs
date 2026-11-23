using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace PriceComparison;

internal static class Program
{
    private static readonly List<WishItem> Wishes =
    [
        new("Lenovo IdeaPad 5", 32000m),
        new("Samsung Galaxy A55", 17000m),
        new("iPhone 15", 40000m),
        new("JBL Tune 520BT", 1500m),
        new("Anker PowerCore 20000", 1600m),
    ];

    private static List<Offer> _offers = [];

    private static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        bool isRunning = true;
        while (isRunning)
        {
            PrintMenu();
            int choice = ReadInt("Your choice: ", 0, 8);
            Console.WriteLine();

            if (choice >= 3 && _offers.Count == 0)
            {
                Console.WriteLine("No data. Load offers first (item 1 or 2).");
                Console.WriteLine();
                continue;
            }

            switch (choice)
            {
                case 1:
                    await LoadParallelAsync();
                    break;
                case 2:
                    await LoadSequentialAsync();
                    break;
                case 3:
                    ShowCheapestOffers();
                    break;
                case 4:
                    ShowCategoryStatistics();
                    break;
                case 5:
                    SearchByPriceRange();
                    break;
                case 6:
                    ShowWishList();
                    break;
                case 7:
                    await SaveReportAsync();
                    break;
                case 8:
                    ShowProductsInAllShops();
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
        Console.WriteLine("===== Price comparison =====");
        Console.WriteLine("1. Load offers (parallel)");
        Console.WriteLine("2. Load offers (sequential)");
        Console.WriteLine("3. Cheapest offer per product");
        Console.WriteLine("4. Category statistics");
        Console.WriteLine("5. Search by price range");
        Console.WriteLine("6. Wish list");
        Console.WriteLine("7. Save report to JSON");
        Console.WriteLine("8. Products sold in all shops");
        Console.WriteLine("0. Exit");
    }

    private static async Task LoadParallelAsync()
    {
        Console.WriteLine("Loading offers in parallel...");
        var stopwatch = Stopwatch.StartNew();

        List<Offer>[] results = await Task.WhenAll(
            ShopSources.LoadTechnoMarketAsync(),
            ShopSources.LoadTsyfraPlusAsync(),
            ShopSources.LoadElektrosvitAsync());

        stopwatch.Stop();
        _offers = results.SelectMany(list => list).ToList();
        Console.WriteLine($"Loaded {_offers.Count} offers in {stopwatch.Elapsed.TotalSeconds:F2} s (parallel).");
    }

    private static async Task LoadSequentialAsync()
    {
        Console.WriteLine("Loading offers one by one...");
        var stopwatch = Stopwatch.StartNew();

        List<Offer> techno = await ShopSources.LoadTechnoMarketAsync();
        List<Offer> tsyfra = await ShopSources.LoadTsyfraPlusAsync();
        List<Offer> elektro = await ShopSources.LoadElektrosvitAsync();

        stopwatch.Stop();
        _offers = [.. techno, .. tsyfra, .. elektro];
        Console.WriteLine($"Loaded {_offers.Count} offers in {stopwatch.Elapsed.TotalSeconds:F2} s (sequential).");
    }

    private static void ShowCheapestOffers()
    {
        Console.WriteLine("--- Cheapest in-stock offer per product ---");
        foreach (CheapestOffer item in PriceQueries.GetCheapestOffers(_offers))
        {
            string where = item.Price is null
                ? "OUT OF STOCK in all shops"
                : $"{item.Price,9:F2} UAH  {item.Shop}";
            Console.WriteLine($"{item.Category,-10} {item.Product,-23} {where}");
        }
    }

    private static void ShowCategoryStatistics()
    {
        var stats = _offers
            .GroupBy(o => o.Category)
            .Select(g => new
            {
                Category = g.Key,
                Count = g.Count(),
                Min = g.Min(o => o.Price),
                Average = g.Average(o => o.Price),
                Max = g.Max(o => o.Price),
            })
            .OrderBy(s => s.Category)
            .ToList();

        Console.WriteLine("--- Category statistics (all offers) ---");
        Console.WriteLine($"{"Category",-10} {"Count",5} {"Min",10} {"Average",10} {"Max",10}");
        foreach (var s in stats)
        {
            string check = s.Min <= s.Max ? string.Empty : "  (error: min > max)";
            Console.WriteLine($"{s.Category,-10} {s.Count,5} {s.Min,10:F2} {s.Average,10:F2} {s.Max,10:F2}{check}");
        }
    }

    private static void SearchByPriceRange()
    {
        decimal min = ReadDecimal("Min price: ");
        decimal max = ReadDecimal("Max price: ");
        if (min > max)
        {
            Console.WriteLine("Min price cannot be greater than max price.");
            return;
        }

        var found = _offers
            .Where(o => o.InStock && o.Price >= min && o.Price <= max)
            .OrderBy(o => o.Price)
            .ThenBy(o => o.Shop)
            .Select(o => new { o.Product, o.Shop, o.Price })
            .ToList();

        if (found.Count == 0)
        {
            Console.WriteLine($"No in-stock offers from {min:F2} to {max:F2} UAH.");
            return;
        }

        Console.WriteLine($"--- In-stock offers from {min:F2} to {max:F2} UAH ---");
        foreach (var o in found)
        {
            Console.WriteLine($"{o.Price,9:F2} UAH  {o.Shop,-12} {o.Product}");
        }
    }

    private static void ShowWishList()
    {
        // Query syntax: join ... into keeps wishes without matching offers.
        var wishResults =
            from w in Wishes
            join o in _offers on w.Product equals o.Product into productOffers
            select new
            {
                Wish = w,
                Matches = productOffers
                    .Where(o => o.InStock && o.Price <= w.MaxPrice)
                    .OrderBy(o => o.Price)
                    .ToList(),
            };

        Console.WriteLine("--- Wish list ---");
        foreach (var result in wishResults)
        {
            Console.WriteLine($"{result.Wish.Product} (up to {result.Wish.MaxPrice:F2} UAH):");
            if (result.Matches.Count == 0)
            {
                Console.WriteLine("    nothing found");
                continue;
            }

            foreach (Offer o in result.Matches)
            {
                Console.WriteLine($"    {o.Price,9:F2} UAH  {o.Shop}");
            }
        }
    }

    private static async Task SaveReportAsync()
    {
        Console.Write("File name (Enter = report.json): ");
        string input = (Console.ReadLine() ?? string.Empty).Trim();
        string fileName = input.Length == 0 ? "report.json" : input;

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };
        string json = JsonSerializer.Serialize(PriceQueries.GetCheapestOffers(_offers), options);

        try
        {
            await File.WriteAllTextAsync(fileName, json);
            Console.WriteLine($"Report saved: {Path.GetFullPath(fileName)}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Could not save the report: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"No access to the file: {ex.Message}");
        }
    }

    private static void ShowProductsInAllShops()
    {
        List<string> products = PriceQueries.GetProductsInAllShops(_offers, ShopSources.ShopNames);
        Console.WriteLine($"--- Products sold in all {ShopSources.ShopNames.Length} shops ---");
        products.ForEach(name => Console.WriteLine($"  {name}"));
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

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? string.Empty).Replace(',', '.');
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Invalid input. Enter a non-negative number, e.g. 1500");
        }
    }
}
