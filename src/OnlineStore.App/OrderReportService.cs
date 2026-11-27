using OnlineStore.Domain;

namespace OnlineStore.App;

public static class OrderReportService
{
    public static List<OrderReport> CreateReport(
        IEnumerable<Order> orders)
    {
        ArgumentNullException.ThrowIfNull(orders);

        return orders.Select(order => new OrderReport(
            order.Id,
            order.Customer.Name,
            order.Total,
            order.Status.ToString(),
            order.Items.Count
        )).ToList();
    }
}