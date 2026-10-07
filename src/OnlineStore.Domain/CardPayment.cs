namespace OnlineStore.Domain;

public class CardPayment : IPaymentMethod
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Payment by card: {amount}");
    }
}