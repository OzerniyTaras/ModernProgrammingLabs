using OnlineStore.Domain;
using OnlineStore.App;

Console.WriteLine("Online Store - Laboratory Work 7");
Console.WriteLine("JSON, Files and Configuration");
Console.WriteLine("========================================");

var customer1 = new Customer
{
    Id = 1,
    Name = "Taras",
    Email = "taras@gmail.com",
    Address = new Address(
        "Poltava", "Shevchenka", "10", "36000"),
    PhoneNumber = null
};

var customer2 = new Customer
{
    Id = 2,
    Name = "Anna",
    Email = "anna@gmail.com",
    Address = new Address(
        "Kyiv", "Khreshchatyk", "15", "01001"),
    PhoneNumber = null
};

var customer3 = new Customer
{
    Id = 3,
    Name = "Ivan",
    Email = "ivan@gmail.com",
    Address = new Address(
        "Lviv", "Franko", "20", "79000"),
    PhoneNumber = null
};

var laptop = new Product(1, "Laptop", 28000m);
var mouse = new Product(2, "Mouse", 600m);
var keyboard = new Product(3, "Keyboard", 1500m);
var monitor = new Product(4, "Monitor", 8500m);
var headphones = new Product(5, "Headphones", 2200m);

// Order 1
var order1 = new Order(1, customer1);
order1.AddItem(laptop, 1);
order1.AddItem(mouse, 2);
order1.Pay(new CardPayment());

// Order 2
var order2 = new Order(2, customer2);
order2.AddItem(keyboard, 1);
order2.AddItem(mouse, 1);
order2.Pay(new CardPayment());

// Order 3
var order3 = new Order(3, customer3);
order3.AddItem(monitor, 2);

// Order 4
var order4 = new Order(4, customer1);
order4.AddItem(headphones, 1);
order4.Cancel();

// Order 5
var order5 = new Order(5, customer2);
order5.AddItem(laptop, 1);
order5.AddItem(monitor, 1);
order5.AddItem(keyboard, 1);
order5.Pay(new CardPayment());

var orders = new List<Order>
{
    order1,
    order2,
    order3,
    order4,
    order5
};

// Create report
var reports = OrderReportService.CreateReport(orders);

Console.WriteLine("\n========================================");
Console.WriteLine("Order Report");
Console.WriteLine("========================================");

foreach (var report in reports)
{
    Console.WriteLine($"Order ID: {report.OrderId}");
    Console.WriteLine($"Customer: {report.CustomerName}");
    Console.WriteLine($"Total: {report.Total:F2}");
    Console.WriteLine($"Status: {report.Status}");
    Console.WriteLine($"Items count: {report.ItemsCount}");
    Console.WriteLine("----------------------------------------");
}

Console.WriteLine($"Total orders: {reports.Count}");
Console.WriteLine($"Total amount: {reports.Sum(r => r.Total):F2}");

Console.WriteLine("\nDemonstration completed.");