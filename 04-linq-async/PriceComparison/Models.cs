namespace PriceComparison;

public record Offer(string Shop, string Product, string Category, decimal Price, bool InStock);

public record WishItem(string Product, decimal MaxPrice);

// Row of the JSON report: the cheapest in-stock offer for one product.
public record CheapestOffer(string Product, string Category, string? Shop, decimal? Price, string Status);
