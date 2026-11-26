
using OnlineStore.Domain;

namespace OnlineStore.App;

public static class OrderQueries
{
    public static void Run(IEnumerable<Order> orders)
    {
        // 1. Orders with total greater than 1000
        Console.WriteLine("\n1. Orders with total > 1000:");

        var expensiveOrders = orders
            .Where(o => o.Total > 1000m);

        foreach (var order in expensiveOrders)
        {
            Console.WriteLine($"Order #{order.Id}: {order.Total}");
        }

        // 2. Top 5 most expensive orders
        Console.WriteLine("\n2. Top 5 most expensive orders:");

        var topOrders = orders
            .OrderByDescending(o => o.Total)
            .Take(5);

        foreach (var order in topOrders)
        {
            Console.WriteLine($"Order #{order.Id}: {order.Total}");
        }

        // 3. Total sales
        Console.WriteLine("\n3. Total sales:");

        var totalSales = orders
            .Sum(o => o.Total);

        Console.WriteLine(totalSales);

        // 4. Average order value
        Console.WriteLine("\n4. Average order value:");

        var averageOrder = orders
            .Average(o => o.Total);

        Console.WriteLine($"{averageOrder:F2}");

        // 5. Number of orders per customer
        Console.WriteLine("\n5. Orders per customer:");

        var ordersByCustomer = orders
            .GroupBy(o => o.Customer.Id)
            .Select(g => new
            {
                Customer = g.First().Customer.Name,
                Count = g.Count()
            });

        foreach (var customer in ordersByCustomer)
        {
            Console.WriteLine($"{customer.Customer}: {customer.Count}");
        }

        // 6. All products from all orders
        Console.WriteLine("\n6. All products from all orders:");

        var allProducts = orders
            .SelectMany(o => o.Items)
            .Select(i => i.Product);

        foreach (var product in allProducts)
        {
            Console.WriteLine(product.Name);
        }

        // 7. Top 3 products by quantity sold
        Console.WriteLine("\n7. Top 3 products by quantity:");

        var topProducts = orders
            .SelectMany(o => o.Items)
            .GroupBy(i => i.Product.Id)
            .Select(g => new
            {
                Product = g.First().Product.Name,
                Quantity = g.Sum(i => i.Quantity)
            })
            .OrderByDescending(p => p.Quantity)
            .Take(3);

        foreach (var product in topProducts)
        {
            Console.WriteLine($"{product.Product}: {product.Quantity}");
        }

        // 8. Customers who spent more than 5000
        Console.WriteLine("\n8. Customers who spent > 5000:");

        var activeCustomers = orders
            .GroupBy(o => o.Customer.Id)
            .Select(g => new
            {
                Customer = g.First().Customer.Name,
                TotalSpent = g.Sum(o => o.Total)
            })
            .Where(c => c.TotalSpent > 5000m);

        foreach (var customer in activeCustomers)
        {
            Console.WriteLine($"{customer.Customer}: {customer.TotalSpent}");
        }

        // 9. Check if any order exceeds 10000
        Console.WriteLine("\n9. Any order > 10000:");

        bool hasExpensiveOrder = orders
            .Any(o => o.Total > 10000m);

        Console.WriteLine(hasExpensiveOrder);

        // 10. Dictionary of customer expenses
        Console.WriteLine("\n10. Customer expenses dictionary:");

        Dictionary<int, decimal> customerExpenses = orders
            .GroupBy(o => o.Customer.Id)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(o => o.Total));

        foreach (var customer in customerExpenses)
        {
            Console.WriteLine(
                $"Customer ID: {customer.Key}, Total: {customer.Value}");
        }
    }
}
