using OnlineStore.Domain;

Console.WriteLine("Online Store - Lab 3");
Console.WriteLine();

var product1 = new Product(1, "Keyboard", 1200m);
var product2 = new Product(2, "Mouse", 600m);
var product3 = new Product(3, "Mouse Pad", 300m);

var address = new Address(
    "Poltava",
    "Shevchenka Street",
    "10",
    "36000"
);

var customer = new Customer
{
    Id = 1,
    Name = "Taras",
    Email = "taras@example.com",
    Address = address,
    PhoneNumber = null
};

Console.WriteLine($"Customer: {customer.Name}");
Console.WriteLine($"Email: {customer.Email}");
Console.WriteLine($"City: {customer.Address.City}");
Console.WriteLine($"Street: {customer.Address.Street}");
Console.WriteLine($"Building: {customer.Address.Building}");
Console.WriteLine($"Postal code: {customer.Address.PostalCode}");
Console.WriteLine($"Phone: {customer.PhoneNumber ?? "Not specified"}");

Console.WriteLine();

// Record and with expression
var newAddress = address with { Building = "15" };

Console.WriteLine($"Original building: {address.Building}");
Console.WriteLine($"Updated building: {newAddress.Building}");

Console.WriteLine();

// Order 1 - Medium, free delivery
var order1 = new Order(1, customer);

order1.AddItem(product1, 1);
order1.AddItem(product2, 2);

IPaymentMethod paymentMethod = new CardPayment();
order1.Pay(paymentMethod);

PrintOrder(order1);

// Order 2 - Medium, standard delivery
var order2 = new Order(2, customer);

order2.AddItem(product2, 1);

paymentMethod = new BankTransferPayment();
order2.Pay(paymentMethod);

PrintOrder(order2);

// Order 3 - Small
var order3 = new Order(3, customer);

order3.AddItem(product3, 1);

PrintOrder(order3);

// Order 4 - Large
var order4 = new Order(4, customer);

order4.AddItem(product1, 5);
order4.Pay(new CardPayment());

PrintOrder(order4);

// Order 5 - Cancelled
var order5 = new Order(5, customer);

order5.AddItem(product2, 1);
order5.Cancel();

PrintOrder(order5);

static void PrintOrder(Order order)
{
    Console.WriteLine($"Order #{order.Id}");
    Console.WriteLine($"Total: {order.Total}");
    Console.WriteLine($"Status: {order.GetStatusDescription()}");
    Console.WriteLine($"Category: {order.GetOrderCategory()}");

    Console.WriteLine(
        $"Delivery: {(order.HasFreeDelivery()
            ? "Free Delivery"
            : "Standard Delivery")}");

    Console.WriteLine("------------------------------");
}
