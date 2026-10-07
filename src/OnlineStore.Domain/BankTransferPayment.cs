namespace OnlineStore.Domain;

public class BankTransferPayment : IPaymentMethod
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Payment by bank transfer: {amount}");
    }
}