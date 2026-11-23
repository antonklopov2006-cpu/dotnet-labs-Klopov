namespace PriceComparison;

public static class PriceQueries
{
    // Item 3. Cheapest in-stock offer for every product.
    // Products that are out of stock everywhere stay in the result with a marker.
    public static List<CheapestOffer> GetCheapestOffers(IEnumerable<Offer> offers) =>
        offers
            .GroupBy(o => o.Product)
            .Select(g =>
            {
                Offer? best = g.Any(o => o.InStock)
                    ? g.Where(o => o.InStock).MinBy(o => o.Price)
                    : null;

                return best is null
                    ? new CheapestOffer(g.Key, g.First().Category, null, null, "немає в наявності")
                    : new CheapestOffer(g.Key, best.Category, best.Shop, best.Price, "в наявності");
            })
            .OrderBy(c => c.Category)
            .ThenBy(c => c.Product)
            .ToList();

    // Item 6. Products that are sold in every shop (checked with All).
    public static List<string> GetProductsInAllShops(IEnumerable<Offer> offers, string[] shopNames) =>
        offers
            .GroupBy(o => o.Product)
            .Where(g => shopNames.All(shop => g.Any(o => o.Shop == shop)))
            .Select(g => g.Key)
            .OrderBy(name => name)
            .ToList();
}
