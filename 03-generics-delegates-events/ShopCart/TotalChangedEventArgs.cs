namespace ShopCart;

public class TotalChangedEventArgs : EventArgs
{
    public TotalChangedEventArgs(decimal oldTotal, decimal newTotal)
    {
        OldTotal = oldTotal;
        NewTotal = newTotal;
    }

    public decimal OldTotal { get; }

    public decimal NewTotal { get; }
}
