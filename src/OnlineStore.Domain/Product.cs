namespace OnlineStore.Domain;

public class Product
{
    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    public Product(int id, string name, decimal price)
    {
        if (price <= 0)
        {
            throw new ArgumentException("Product price must be greater than 0.");
        }

        Id = id;
        Name = name;
        Price = price;
    }
}