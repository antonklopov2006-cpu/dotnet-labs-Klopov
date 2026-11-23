# Practical work 3. Generics, delegates, events and lambdas

Variant 1 - Online shop cart.

- `Cart<T> where T : IPriced` with `Add`, `Remove`, `GetTotal`, `ForEach(Action<T>)`, `SetDiscount(Func<decimal, decimal>?)`
- Promo codes `SALE10`, `MINUS100`, `BIG20` in `Dictionary<string, Func<decimal, decimal>>`
- Event `TotalChanged` (`TotalChangedEventArgs`), two subscribers: named method and lambda
- Extras: custom percent discount (closure), `Cart<Ticket>`

Run: open `ShopCart.sln` in Visual Studio. Requires .NET 10 SDK.
