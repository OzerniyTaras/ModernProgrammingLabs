namespace OnlineStore.Domain;

public class OrderItem
{
    public Product Product { get; }
    public int Quantity { get; }

    public OrderItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        Product = product;
        Quantity = quantity;
    }

    public decimal GetTotalPrice()
    {
        return Product.Price * Quantity;
    }
}