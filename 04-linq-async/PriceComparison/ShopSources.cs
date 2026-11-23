namespace PriceComparison;

// Each method simulates a request to a shop server with its own response time.
public static class ShopSources
{
    public static readonly string[] ShopNames = ["ТехноМаркет", "ЦифраПлюс", "Електросвіт"];

    public static async Task<List<Offer>> LoadTechnoMarketAsync()
    {
        await Task.Delay(1000);
        const string shop = "ТехноМаркет";
        List<Offer> offers =
        [
            new(shop, "Lenovo IdeaPad 5", "Ноутбуки", 32999m, true),
            new(shop, "ASUS VivoBook 15", "Ноутбуки", 24999m, true),
            new(shop, "HP Pavilion 14", "Ноутбуки", 28499m, false),
            new(shop, "Samsung Galaxy A55", "Смартфони", 16999m, true),
            new(shop, "iPhone 15", "Смартфони", 37999m, false),
            new(shop, "Xiaomi Redmi Note 13", "Смартфони", 8999m, true),
            new(shop, "Logitech MX Master 3S", "Аксесуари", 4299m, true),
            new(shop, "JBL Tune 520BT", "Аксесуари", 1999m, true),
            new(shop, "Anker PowerCore 20000", "Аксесуари", 1599m, false),
        ];
        Console.WriteLine($"  {shop}: {offers.Count} offers received");
        return offers;
    }

    public static async Task<List<Offer>> LoadTsyfraPlusAsync()
    {
        await Task.Delay(2000);
        const string shop = "ЦифраПлюс";
        List<Offer> offers =
        [
            new(shop, "Lenovo IdeaPad 5", "Ноутбуки", 31499m, true),
            new(shop, "HP Pavilion 14", "Ноутбуки", 27999m, true),
            new(shop, "Samsung Galaxy A55", "Смартфони", 17499m, true),
            new(shop, "iPhone 15", "Смартфони", 36999m, false),
            new(shop, "Xiaomi Redmi Note 13", "Смартфони", 9299m, false),
            new(shop, "Logitech MX Master 3S", "Аксесуари", 3999m, true),
            new(shop, "JBL Tune 520BT", "Аксесуари", 2199m, true),
            new(shop, "Anker PowerCore 20000", "Аксесуари", 1499m, true),
        ];
        Console.WriteLine($"  {shop}: {offers.Count} offers received");
        return offers;
    }

    public static async Task<List<Offer>> LoadElektrosvitAsync()
    {
        await Task.Delay(1500);
        const string shop = "Електросвіт";
        List<Offer> offers =
        [
            new(shop, "Lenovo IdeaPad 5", "Ноутбуки", 33499m, false),
            new(shop, "ASUS VivoBook 15", "Ноутбуки", 23999m, true),
            new(shop, "HP Pavilion 14", "Ноутбуки", 28999m, true),
            new(shop, "Samsung Galaxy A55", "Смартфони", 16499m, false),
            new(shop, "iPhone 15", "Смартфони", 38499m, false),
            new(shop, "Xiaomi Redmi Note 13", "Смартфони", 8799m, true),
            new(shop, "JBL Tune 520BT", "Аксесуари", 1899m, true),
            new(shop, "Anker PowerCore 20000", "Аксесуари", 1549m, true),
        ];
        Console.WriteLine($"  {shop}: {offers.Count} offers received");
        return offers;
    }
}
