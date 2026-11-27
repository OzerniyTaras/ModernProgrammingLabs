namespace OnlineStore.Domain;

public class Product
{
    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    public Product(int id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        Id = id;
        Name = name;
        Price = price;
    }
}