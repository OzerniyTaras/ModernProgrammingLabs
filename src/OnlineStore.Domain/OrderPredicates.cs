
namespace OnlineStore.Domain;

public static class OrderPredicates
{
    public static readonly Predicate<Order> IsExpensive =
        order => order.Total >= 5000m;
}
