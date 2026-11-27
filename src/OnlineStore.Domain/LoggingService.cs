namespace OnlineStore.Domain;

public class LoggingService
{
    public void OnOrderPaid(Order order)
    {
        Console.WriteLine(
            $"[LOGGING] Order #{order.Id} paid. " +
            $"Customer: {order.Customer.Name}. " +
            $"Total: {order.Total}");
    }
}