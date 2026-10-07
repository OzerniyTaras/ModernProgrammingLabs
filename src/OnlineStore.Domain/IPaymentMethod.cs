namespace OnlineStore.Domain;

public interface IPaymentMethod
{
    void Pay(decimal amount);
}