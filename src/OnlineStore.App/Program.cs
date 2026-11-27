using OnlineStore.Domain;

Console.WriteLine("Online Store - Laboratory Work 6");
Console.WriteLine("Validation and Exception Handling");
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

// 1. Valid operation
RunOperation("Add valid products", () =>
{
    order.AddItem(laptop, 1);
    order.AddItem(mouse, 2);
});

PrintOrderState(order);

// 2. Invalid product name
RunOperation("Create product with empty name", () =>
{
    var product = new Product(3, "", 100m);
});

// 3. Invalid product price
RunOperation("Create product with negative price", () =>
{
    var product = new Product(3, "Keyboard", -100m);
});

// 4. Invalid quantity
RunOperation("Add product with negative quantity", () =>
{
    order.AddItem(mouse, -2);
});

PrintOrderState(order);

// 5. Successful payment
var notificationService = new NotificationService();
var loggingService = new LoggingService();

order.OrderPaid += notificationService.OnOrderPaid;
order.OrderPaid += loggingService.OnOrderPaid;

RunOperation("Pay order", () =>
{
    order.Pay(new CardPayment());
});

PrintOrderState(order);

// 6. Repeated payment
RunOperation("Pay the same order again", () =>
{
    order.Pay(new CardPayment());
});

// 7. Modify paid order
RunOperation("Add item to paid order", () =>
{
    order.AddItem(mouse, 1);
});

RunOperation("Remove item from paid order", () =>
{
    order.RemoveItem(laptop);
});

PrintOrderState(order);

// 8. Cancelled order
var cancelledOrder = new Order(2, customer);
cancelledOrder.AddItem(mouse, 1);
cancelledOrder.Cancel();

RunOperation("Pay cancelled order", () =>
{
    cancelledOrder.Pay(new CardPayment());
});

RunOperation("Modify cancelled order", () =>
{
    cancelledOrder.AddItem(laptop, 1);
});

PrintOrderState(cancelledOrder);

// 9. Custom domain exception
var emptyOrder = new Order(3, customer);

RunOperation("Pay empty order", () =>
{
    emptyOrder.Pay(new CardPayment());
});

PrintOrderState(emptyOrder);

// 10. Failed payment and event verification
Console.WriteLine("\n========================================");
Console.WriteLine("Failed Payment and Event Verification");
Console.WriteLine("========================================");

var failedOrder = new Order(4, customer);
failedOrder.AddItem(laptop, 1);
failedOrder.AddItem(mouse, 2);

int eventCount = 0;

failedOrder.OrderPaid += notificationService.OnOrderPaid;
failedOrder.OrderPaid += loggingService.OnOrderPaid;
failedOrder.OrderPaid += _ => eventCount++;

var statusBefore = failedOrder.Status;
var countBefore = failedOrder.Items.Count;
var totalBefore = failedOrder.Total;

Console.WriteLine("\nBefore failed payment:");
PrintOrderState(failedOrder);
Console.WriteLine($"OrderPaid event count: {eventCount}");

RunOperation("Attempt declined payment", () =>
{
    failedOrder.Pay(new FailedPayment());
});

Console.WriteLine("\nAfter failed payment:");
PrintOrderState(failedOrder);
Console.WriteLine($"OrderPaid event count: {eventCount}");

bool stateUnchanged =
    failedOrder.Status == statusBefore &&
    failedOrder.Items.Count == countBefore &&
    failedOrder.Total == totalBefore;

bool eventNotRaised = eventCount == 0;

Console.WriteLine("\nVerification results:");
Console.WriteLine($"Order state unchanged: {stateUnchanged}");
Console.WriteLine($"OrderPaid event not raised: {eventNotRaised}");

Console.WriteLine(
    $"Failed payment test: " +
    $"{(stateUnchanged && eventNotRaised ? "PASSED" : "FAILED")}");

// Successful payment after failed attempt
RunOperation("Retry payment successfully", () =>
{
    failedOrder.Pay(new CardPayment());
});

PrintOrderState(failedOrder);
Console.WriteLine($"OrderPaid event count: {eventCount}");

bool retrySuccessful =
    failedOrder.Status == OrderStatus.Paid &&
    eventCount == 1;

Console.WriteLine(
    $"Successful retry test: " +
    $"{(retrySuccessful ? "PASSED" : "FAILED")}");

// 11. TryParse demonstration
Console.WriteLine("\n========================================");
Console.WriteLine("TryParse - User Input");
Console.WriteLine("========================================");

var inputOrder = new Order(5, customer);

while (true)
{
    Console.Write("Enter mouse quantity: ");

    string? input = Console.ReadLine();

    if (input is null)
    {
        Console.WriteLine("Input ended.");
        break;
    }

    if (!int.TryParse(input, out int quantity))
    {
        Console.WriteLine("Error: Enter a valid integer.");
        continue;
    }

    if (quantity <= 0)
    {
        Console.WriteLine(
            "Error: Quantity must be greater than zero.");
        continue;
    }

    RunOperation("Add product using user input", () =>
    {
        inputOrder.AddItem(mouse, quantity);
    });

    PrintOrderState(inputOrder);
    break;
}

Console.WriteLine("\n========================================");
Console.WriteLine("Demonstration completed.");

static void RunOperation(string operation, Action action)
{
    Console.WriteLine($"\nOperation: {operation}");

    try
    {
        action();
        Console.WriteLine("Result: Success");
    }
    catch (DomainRuleViolationException ex)
    {
        Console.WriteLine($"Domain rule error: {ex.Message}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Range error: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Argument error: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Operation error: {ex.Message}");
    }
}

static void PrintOrderState(Order order)
{
    Console.WriteLine($"Order ID: {order.Id}");
    Console.WriteLine($"Order status: {order.Status}");
    Console.WriteLine($"Order items count: {order.Items.Count}");
    Console.WriteLine($"Order total: {order.Total:F2}");
}