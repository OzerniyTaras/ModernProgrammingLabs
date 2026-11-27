namespace OnlineStore.Domain;

public class NotificationService
{
    public void OnOrderPaid(Order order)
    {
        Console.WriteLine(
            $"[NOTIFICATION] Customer {order.Customer.Name}, " +
            $"your order #{order.Id} has been paid successfully.");

        Console.WriteLine(
            $"[NOTIFICATION] Email: {order.Customer.Email}");
    }
}