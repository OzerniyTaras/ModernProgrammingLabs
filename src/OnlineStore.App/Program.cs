using OnlineStore.Domain;
using OnlineStore.App;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Online Store - Laboratory Work 7");
Console.WriteLine("JSON, Files and Configuration");
Console.WriteLine("========================================");

// Load configuration
string configFile = Path.Combine(
    AppContext.BaseDirectory, "appsettings.json");

var config = new ConfigurationBuilder()
    .AddJsonFile(configFile, optional: false)
    .Build();

string relativePath = config["Storage:FilePath"]
    ?? throw new InvalidOperationException(
        "Storage:FilePath is not configured.");

string filePath = Path.Combine(
    AppContext.BaseDirectory, relativePath);

string dataDirectory = Path.GetDirectoryName(filePath)!;

Console.WriteLine("\nApplication Configuration");
Console.WriteLine("========================================");
Console.WriteLine($"Configuration file: {configFile}");
Console.WriteLine($"Storage:FilePath = {relativePath}");
Console.WriteLine($"Full report path: {filePath}");
Console.WriteLine("Configuration loaded successfully.");
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

// JSON export and import
try
{
    Console.WriteLine("\n========================================");
    Console.WriteLine("JSON File Export");
    Console.WriteLine("========================================");

    await JsonReportStorage.SaveAsync(reports, filePath);

    Console.WriteLine("Report successfully saved to JSON.");
    Console.WriteLine($"File path: {filePath}");
    Console.WriteLine($"Exported records: {reports.Count}");
    Console.WriteLine($"File exists: {File.Exists(filePath)}");

    Console.WriteLine("\n========================================");
    Console.WriteLine("JSON File Import");
    Console.WriteLine("========================================");

    var loadedReports =
        await JsonReportStorage.LoadAsync(filePath);

    Console.WriteLine("Report successfully loaded from JSON.");

    foreach (var report in loadedReports)
    {
        Console.WriteLine(
            $"Order #{report.OrderId} | " +
            $"{report.CustomerName} | " +
            $"{report.Total:F2} | " +
            $"{report.Status} | " +
            $"Items: {report.ItemsCount}");
    }

    Console.WriteLine("----------------------------------------");
    Console.WriteLine($"Loaded records: {loadedReports.Count}");
    Console.WriteLine(
        $"Loaded total amount: {loadedReports.Sum(r => r.Total):F2}");

    bool countMatches =
        reports.Count == loadedReports.Count;

    bool totalMatches =
        reports.Sum(r => r.Total) ==
        loadedReports.Sum(r => r.Total);

    bool dataMatches =
        reports.SequenceEqual(loadedReports);

    Console.WriteLine("\nVerification results:");
    Console.WriteLine($"Record count matches: {countMatches}");
    Console.WriteLine($"Total amount matches: {totalMatches}");
    Console.WriteLine($"All report data matches: {dataMatches}");

    Console.WriteLine(
        $"JSON round-trip test: " +
        $"{(countMatches && totalMatches && dataMatches ? "PASSED" : "FAILED")}");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"File not found: {ex.Message}");
}
catch (JsonException ex)
{
    Console.WriteLine($"JSON error: {ex.Message}");
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"Access denied: {ex.Message}");
}
catch (IOException ex)
{
    Console.WriteLine($"File operation error: {ex.Message}");
}

// Error handling demonstration
Console.WriteLine("\n========================================");
Console.WriteLine("File and JSON Error Handling");
Console.WriteLine("========================================");

// Test 1: Missing file
Console.WriteLine("\nTest 1: Missing JSON file");

string missingFilePath = Path.Combine(
    dataDirectory, $"{Guid.NewGuid():N}.json");

try
{
    await JsonReportStorage.LoadAsync(missingFilePath);

    Console.WriteLine("Missing file test: FAILED");
}
catch (FileNotFoundException)
{
    Console.WriteLine("Error: JSON file was not found.");
    Console.WriteLine("Missing file test: PASSED");
}
catch (DirectoryNotFoundException)
{
    Console.WriteLine("Error: JSON directory was not found.");
    Console.WriteLine("Missing file test: PASSED");
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"Access denied: {ex.Message}");
}
catch (IOException ex)
{
    Console.WriteLine($"File operation error: {ex.Message}");
}

// Test 2: Invalid JSON
Console.WriteLine("\nTest 2: Invalid JSON content");

string invalidFilePath = Path.Combine(
    dataDirectory, "invalid-report.json");

try
{
    Directory.CreateDirectory(dataDirectory);

    await File.WriteAllTextAsync(
        invalidFilePath, "{ invalid json }");

    await JsonReportStorage.LoadAsync(invalidFilePath);

    Console.WriteLine("Invalid JSON test: FAILED");
}
catch (JsonException)
{
    Console.WriteLine("Error: Invalid JSON format.");
    Console.WriteLine("Invalid JSON test: PASSED");
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"Access denied: {ex.Message}");
}
catch (IOException ex)
{
    Console.WriteLine($"File operation error: {ex.Message}");
}

Console.WriteLine("\nProgram continues after error handling.");

Console.WriteLine("\n========================================");
Console.WriteLine("Demonstration completed.");