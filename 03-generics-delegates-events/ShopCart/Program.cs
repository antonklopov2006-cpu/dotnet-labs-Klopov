using System.Globalization;

namespace ShopCart;

internal static class Program
{
    private const decimal FreeShippingThreshold = 1000m;

    private static readonly Dictionary<string, Func<decimal, decimal>> PromoCodes = new()
    {
        ["SALE10"] = total => total * 0.9m,
        ["MINUS100"] = total => Math.Max(0, total - 100),
        ["BIG20"] = total => total >= 2000 ? total * 0.8m : total,
    };

    private static void Main()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        var cart = new Cart<Product>();
        cart.Add(new Product("Wireless mouse", 450m));
        cart.Add(new Product("USB-C cable", 199.99m));
        cart.Add(new Product("Keyboard", 1250m));

        // Subscriber 1: named method.
        cart.TotalChanged += PrintNewTotal;

        // Subscriber 2: lambda.
        cart.TotalChanged += (sender, e) =>
        {
            if (e.NewTotal >= FreeShippingThreshold)
            {
                Console.WriteLine("  [shipping] Free shipping is available!");
            }
            else
            {
                Console.WriteLine($"  [shipping] {FreeShippingThreshold - e.NewTotal:F2} UAH left until free shipping.");
            }
        };

        bool isRunning = true;
        while (isRunning)
        {
            PrintMenu();
            int choice = ReadInt("Your choice: ", 0, 7);
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    AddProduct(cart);
                    break;
                case 2:
                    RemoveProduct(cart);
                    break;
                case 3:
                    ShowCart(cart);
                    break;
                case 4:
                    EnterPromoCode(cart);
                    break;
                case 5:
                    cart.SetDiscount(null);
                    Console.WriteLine("Promo code cancelled.");
                    break;
                case 6:
                    EnterCustomPercent(cart);
                    break;
                case 7:
                    ShowTicketCart();
                    break;
                case 0:
                    isRunning = false;
                    Console.WriteLine("Goodbye!");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void PrintNewTotal(object? sender, TotalChangedEventArgs e)
    {
        Console.WriteLine($"  [cart] Total changed: {e.OldTotal:F2} -> {e.NewTotal:F2} UAH");
    }

    private static void PrintMenu()
    {
        Console.WriteLine("===== Online shop cart =====");
        Console.WriteLine("1. Add a product");
        Console.WriteLine("2. Remove a product by name");
        Console.WriteLine("3. Show the cart");
        Console.WriteLine("4. Enter a promo code");
        Console.WriteLine("5. Cancel the promo code");
        Console.WriteLine("6. Custom percent discount");
        Console.WriteLine("7. Ticket cart demo (Cart<Ticket>)");
        Console.WriteLine("0. Exit");
    }

    private static void AddProduct(Cart<Product> cart)
    {
        Console.Write("Product name: ");
        string name = Console.ReadLine() ?? string.Empty;
        decimal price = ReadDecimal("Price (UAH): ");

        try
        {
            cart.Add(new Product(name, price));
            Console.WriteLine("Product added.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Product was not added: {ex.Message}");
        }
    }

    private static void RemoveProduct(Cart<Product> cart)
    {
        Console.Write("Name of the product to remove: ");
        string name = Console.ReadLine() ?? string.Empty;

        if (cart.Remove(name))
        {
            Console.WriteLine("Product removed.");
        }
        else
        {
            Console.WriteLine($"Product \"{name}\" is not in the cart.");
        }
    }

    private static void ShowCart(Cart<Product> cart)
    {
        if (cart.Count == 0)
        {
            Console.WriteLine("The cart is empty.");
            return;
        }

        Console.WriteLine("--- Cart ---");
        int number = 1;
        cart.ForEach(product => Console.WriteLine($"{number++,2}. {product}"));

        Console.WriteLine($"Subtotal:   {cart.GetSubtotal(),10:F2} UAH");
        Console.WriteLine($"To pay:     {cart.GetTotal(),10:F2} UAH{(cart.HasDiscount ? " (promo applied)" : string.Empty)}");
    }

    private static void EnterPromoCode(Cart<Product> cart)
    {
        Console.Write("Promo code: ");
        string code = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

        if (PromoCodes.TryGetValue(code, out Func<decimal, decimal>? discount))
        {
            cart.SetDiscount(discount);
            Console.WriteLine($"Promo code {code} applied.");
        }
        else
        {
            Console.WriteLine($"Unknown promo code \"{code}\". Available: {string.Join(", ", PromoCodes.Keys)}");
        }
    }

    // The lambda captures the local variable percent (closure).
    private static void EnterCustomPercent(Cart<Product> cart)
    {
        decimal percent = ReadDecimal("Discount percent (1-90): ");
        if (percent < 1 || percent > 90)
        {
            Console.WriteLine("Percent must be from 1 to 90.");
            return;
        }

        cart.SetDiscount(total => total * (100 - percent) / 100);
        Console.WriteLine($"Discount {percent}% applied.");
    }

    private static void ShowTicketCart()
    {
        var tickets = new Cart<Ticket>();
        tickets.Add(new Ticket("Concert", "Row 5, Seat 12", 800m));
        tickets.Add(new Ticket("Concert", "Row 5, Seat 13", 800m));
        tickets.Add(new Ticket("Theatre", "Balcony 2", 450m));

        Console.WriteLine("--- Cart<Ticket> ---");
        tickets.ForEach(ticket => Console.WriteLine($"   {ticket}"));
        Console.WriteLine($"Total: {tickets.GetTotal():F2} UAH");

        tickets.SetDiscount(PromoCodes["BIG20"]);
        Console.WriteLine($"With BIG20: {tickets.GetTotal():F2} UAH");
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
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
            {
                return value;
            }

            Console.WriteLine("Invalid input. Enter a number, e.g. 199.99");
        }
    }
}
