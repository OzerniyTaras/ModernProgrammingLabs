namespace OnlineStore.Domain;

public class Order
{
    private readonly List<OrderItem> _items = [];

    public int Id { get; }
    public Customer Customer { get; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public event Action<Order>? OrderPaid;

    public decimal Total => _items.Sum(
        item => item.GetTotalPrice());

    public Order(int id, Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        Id = id;
        Customer = customer;
        Status = OrderStatus.Created;
    }

    public void AddItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        if (Status != OrderStatus.Created)
        {
            throw new InvalidOperationException(
                "Items can be changed only in a new order.");
        }

        var item = new OrderItem(product, quantity);
        _items.Add(item);

        DomainLogger.Logger(
            $"Order #{Id}: Added {product.Name}, quantity: {quantity}");
    }

    public void RemoveItem(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (Status != OrderStatus.Created)
        {
            throw new InvalidOperationException(
                "Items can be changed only in a new order.");
        }

        var item = _items.FirstOrDefault(
            x => x.Product.Id == product.Id);

        if (item != null)
        {
            _items.Remove(item);
        }
    }

    public void Pay(IPaymentMethod paymentMethod)
    {
        ArgumentNullException.ThrowIfNull(paymentMethod);

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

        if (Status != OrderStatus.Created)
        {
            throw new InvalidOperationException(
                "Only a new order can be paid.");
        }

        if (_items.Count == 0)
        {
            throw new DomainRuleViolationException(
                "An empty order cannot be paid.");
        }

        paymentMethod.Pay(Total);

        Status = OrderStatus.Paid;

        DomainLogger.Logger(
            $"Order #{Id}: Payment completed. Total: {Total}");

        OrderPaid?.Invoke(this);
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Created)
        {
            throw new InvalidOperationException(
                "Only a new order can be cancelled.");
        }

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