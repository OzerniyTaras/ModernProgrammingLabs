namespace OnlineStore.Domain;

public class OrderItem
{
    public Product Product { get; }
    public int Quantity { get; }

    public OrderItem(Product product, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than 0.");
        }

        Product = product;
        Quantity = quantity;
    }

    public decimal GetTotalPrice()
    {
        return Product.Price * Quantity;
    }
}