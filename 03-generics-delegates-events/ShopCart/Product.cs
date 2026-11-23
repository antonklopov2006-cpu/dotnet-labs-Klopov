namespace ShopCart;

public class Product : IPriced
{
    private string _name = string.Empty;
    private decimal _price;

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(Name));
            }

            _name = value.Trim();
        }
    }

    public decimal Price
    {
        get => _price;
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.", nameof(Price));
            }

            _price = value;
        }
    }

    public override string ToString() => $"{Name,-22} {Price,10:F2} UAH";
}
