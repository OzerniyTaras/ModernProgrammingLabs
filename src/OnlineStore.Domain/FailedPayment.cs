namespace OnlineStore.Domain;

public class FailedPayment : IPaymentMethod
{
    public void Pay(decimal amount)
    {
        throw new InvalidOperationException(
            "Payment was declined by the payment provider.");
    }
}   