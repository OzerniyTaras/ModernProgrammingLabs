using OnlineStore.Domain;

Console.WriteLine("Online Store - Laboratory Work 5");
Console.WriteLine("Delegates, Lambda Expressions and Events");
Console.WriteLine("========================================");

var customer = new Customer
{
    Id = 1,
    Name = "Taras",
    Email = "taras@gmail.com",
    Address = new Address(
        "Poltava", "Shevchenka", "10", "36000"),
    PhoneNumber = null
};

var laptop = new Product(1, "Laptop", 28000m);
var mouse = new Product(2, "Mouse", 600m);

var order = new Order(1, customer);

// 1. Action
Console.WriteLine("\n1. Action - domain operations");

order.AddItem(laptop, 1);
order.AddItem(mouse, 2);

// 2. Func and lambda expressions
Console.WriteLine("\n2. Func - discount policies");

Console.WriteLine($"Original total: {order.Total:F2}");

Console.WriteLine($"No discount: " +
    $"{DiscountPolicies.ApplyDiscount(order.Total,
        DiscountPolicies.NoDiscount):F2}");

Console.WriteLine($"10% discount: " +
    $"{DiscountPolicies.ApplyDiscount(order.Total,
        DiscountPolicies.TenPercentDiscount):F2}");

Console.WriteLine($"20% discount: " +
    $"{DiscountPolicies.ApplyDiscount(order.Total,
        DiscountPolicies.TwentyPercentDiscount):F2}");

// 3. Predicate
Console.WriteLine("\n3. Predicate - expensive orders");

bool isExpensive = OrderPredicates.IsExpensive(order);

Console.WriteLine($"Order total >= 5000: {isExpensive}");

// 4. Events and subscribers
Console.WriteLine("\n4. Event - OrderPaid");

var notificationService = new NotificationService();
var loggingService = new LoggingService();

order.OrderPaid += notificationService.OnOrderPaid;
order.OrderPaid += loggingService.OnOrderPaid;

order.Pay(new CardPayment());

Console.WriteLine($"Order status: {order.Status}");

order.OrderPaid -= notificationService.OnOrderPaid;
order.OrderPaid -= loggingService.OnOrderPaid;

// 5. Cancellation
Console.WriteLine("\n5. Action - order cancellation");

var secondOrder = new Order(2, customer);

secondOrder.AddItem(mouse, 1);
secondOrder.Cancel();

Console.WriteLine($"Order status: {secondOrder.Status}");

// 6. Extension methods
Console.WriteLine("\n6. Extension Methods");

Console.WriteLine($"Order #1 is large: {order.IsLargeOrder()}");
Console.WriteLine(order.GetSummary());

Console.WriteLine($"Order #2 is large: {secondOrder.IsLargeOrder()}");
Console.WriteLine(secondOrder.GetSummary());

// 7. C# 14 Extension Blocks
Console.WriteLine("\n7. C# 14 Extension Blocks");

Console.WriteLine($"Order #1 is paid: {order.IsPaid()}");
Console.WriteLine($"Order #2 is paid: {secondOrder.IsPaid()}");

Console.WriteLine("\n========================================");
Console.WriteLine("Demonstration completed.");