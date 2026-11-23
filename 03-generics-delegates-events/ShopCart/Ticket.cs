namespace ShopCart;

// Second type for Cart<T>: shows that the generic cart works with any IPriced.
public class Ticket : IPriced
{
    public Ticket(string eventName, string seat, decimal price)
    {
        Name = $"{eventName} ({seat})";
        Price = price;
    }

    public string Name { get; }

    public decimal Price { get; }

    public override string ToString() => $"Ticket: {Name,-28} {Price,10:F2} UAH";
}
