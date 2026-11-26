
using OnlineStore.App;

Console.WriteLine("Online Store - Laboratory Work 4");
Console.WriteLine("Collections, Generics and LINQ");
Console.WriteLine("================================");

var repository = TestData.CreateOrders();

var orders = repository.GetAll();

Console.WriteLine($"\nTotal orders: {orders.Count}");

OrderQueries.Run(orders);

Console.WriteLine("\n================================");
Console.WriteLine("All LINQ queries completed.");
