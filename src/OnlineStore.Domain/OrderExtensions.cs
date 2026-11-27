namespace OnlineStore.Domain;

public static class OrderExtensions
{
    public static bool IsLargeOrder(this Order order)
    {
        return order.Total >= 5000m;
    }

    public static string GetSummary(this Order order)
    {
        return $"Order #{order.Id} | " +
               $"Customer: {order.Customer.Name} | " +
               $"Total: {order.Total:F2} | " +
               $"Status: {order.Status}";
    }

    // C# 14 Extension Block
    extension(Order order)
    {
        public bool IsPaid()
        {
            return order.Status == OrderStatus.Paid;
        }
    }
}