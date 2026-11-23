namespace ShopCart;

public class Cart<T> where T : IPriced
{
    private readonly List<T> _items = [];

    // Optional rule: takes the items' sum and returns the amount to pay.
    private Func<decimal, decimal>? _discount;

    public event EventHandler<TotalChangedEventArgs>? TotalChanged;

    public int Count => _items.Count;

    public bool HasDiscount => _discount is not null;

    public void Add(T item)
    {
        decimal oldTotal = GetTotal();
        _items.Add(item);
        OnTotalChanged(new TotalChangedEventArgs(oldTotal, GetTotal()));
    }

    public bool Remove(string name)
    {
        decimal oldTotal = GetTotal();
        int index = _items.FindIndex(item => item.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            return false;
        }

        _items.RemoveAt(index);
        OnTotalChanged(new TotalChangedEventArgs(oldTotal, GetTotal()));
        return true;
    }

    // Sum of prices without the discount.
    public decimal GetSubtotal()
    {
        decimal sum = 0;
        foreach (T item in _items)
        {
            sum += item.Price;
        }

        return sum;
    }

    public decimal GetTotal()
    {
        decimal subtotal = GetSubtotal();
        decimal total = _discount?.Invoke(subtotal) ?? subtotal;
        return Math.Round(total, 2);
    }

    public void ForEach(Action<T> action)
    {
        foreach (T item in _items)
        {
            action(item);
        }
    }

    public void SetDiscount(Func<decimal, decimal>? discount)
    {
        decimal oldTotal = GetTotal();
        _discount = discount;
        OnTotalChanged(new TotalChangedEventArgs(oldTotal, GetTotal()));
    }

    protected virtual void OnTotalChanged(TotalChangedEventArgs e)
    {
        TotalChanged?.Invoke(this, e);
    }
}
