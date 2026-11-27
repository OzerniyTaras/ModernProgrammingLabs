
namespace OnlineStore.Domain;

public static class DiscountPolicies
{
    public static readonly Func<decimal, decimal> NoDiscount =
        total => total;

    public static readonly Func<decimal, decimal> TenPercentDiscount =
        total => total * 0.9m;

    public static readonly Func<decimal, decimal> TwentyPercentDiscount =
        total => total * 0.8m;

    public static decimal ApplyDiscount(
        decimal total,
        Func<decimal, decimal> discountPolicy)
    {
        return discountPolicy(total);
    }
}
