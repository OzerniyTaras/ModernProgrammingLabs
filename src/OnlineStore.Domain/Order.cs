namespace OnlineStore.Domain;

public class Order
{
    private readonly List<OrderItem> _items = [];

    public int Id { get; }
    public Customer Customer { get; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items;

    public Order(int id, Customer customer)
    {
        Id = id;
        Customer = customer;
        Status = OrderStatus.New;
    }

    public void AddItem(Product product, int quantity)
    {
        var item = new OrderItem(product, quantity);
        _items.Add(item);
    }

    public void RemoveItem(Product product)
    {
        var item = _items.FirstOrDefault(x => x.Product.Id == product.Id);

        if (item != null)
        {
            _items.Remove(item);
        }
    }

    public void Pay(IPaymentMethod paymentMethod)
    {
        if (Status == OrderStatus.Paid)
        {
            throw new InvalidOperationException("Order is already paid.");
        }

        if (Status == OrderStatus.Cancelled)
        {
            throw new InvalidOperationException("Cancelled order cannot be paid.");
        }

        decimal totalAmount = 0;

        foreach (var item in _items)
        {
            totalAmount += item.GetTotalPrice();
        }

        paymentMethod.Pay(totalAmount);
        Status = OrderStatus.Paid;
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
    }
}