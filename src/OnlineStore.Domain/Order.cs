
namespace OnlineStore.Domain;

public class Order
{
    private readonly List<OrderItem> _items = [];

    public int Id { get; }
    public Customer Customer { get; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items;

    public decimal Total => _items.Sum(
        item => item.GetTotalPrice());

    public Order(int id, Customer customer)
    {
        Id = id;
        Customer = customer;
        Status = OrderStatus.Created;
    }

    public void AddItem(Product product, int quantity)
    {
        var item = new OrderItem(product, quantity);
        _items.Add(item);

        DomainLogger.Logger(
            $"Order #{Id}: Added {product.Name}, quantity: {quantity}");
    }

    public void RemoveItem(Product product)
    {
        var item = _items.FirstOrDefault(
            x => x.Product.Id == product.Id);

        if (item != null)
        {
            _items.Remove(item);
        }
    }

    public void Pay(IPaymentMethod paymentMethod)
    {
        if (Status == OrderStatus.Paid)
        {
            throw new InvalidOperationException(
                "Order is already paid.");
        }

        if (Status == OrderStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "Cancelled order cannot be paid.");
        }

        paymentMethod.Pay(Total);
        Status = OrderStatus.Paid;

        DomainLogger.Logger(
            $"Order #{Id}: Payment completed. Total: {Total}");
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;

        DomainLogger.Logger(
            $"Order #{Id}: Order cancelled.");
    }

    public string GetStatusDescription()
    {
        return Status switch
        {
            OrderStatus.Created => "Order created",
            OrderStatus.Paid => "Order paid",
            OrderStatus.Shipped => "Order shipped",
            OrderStatus.Delivered => "Order delivered",
            OrderStatus.Cancelled => "Order cancelled",
            _ => "Unknown status"
        };
    }

    public string GetOrderCategory()
    {
        return Total switch
        {
            < 500m => "Small",
            < 5000m => "Medium",
            _ => "Large"
        };
    }

    public bool HasFreeDelivery()
    {
        return this is
        {
            Status: OrderStatus.Paid,
            Total: >= 2000m
        };
    }
}
