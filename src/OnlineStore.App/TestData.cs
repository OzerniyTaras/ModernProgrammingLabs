
using OnlineStore.Domain;

namespace OnlineStore.App;

public static class TestData
{
    public static InMemoryRepository<Order> CreateOrders()
    {
        var repository = new InMemoryRepository<Order>();

        Product[] products =
        [
            new Product(1, "Keyboard", 1200m),
            new Product(2, "Mouse", 600m),
            new Product(3, "Monitor", 7500m),
            new Product(4, "Laptop", 28000m),
            new Product(5, "Headphones", 1800m),
            new Product(6, "USB Cable", 300m),
            new Product(7, "Webcam", 2200m),
            new Product(8, "SSD", 3500m)
        ];

        Customer[] customers =
        [
            new Customer
            {
                Id = 1,
                Name = "Taras",
                Email = "taras@gmail.com",
                Address = new Address("Poltava", "Shevchenka", "10", "36000"),
                PhoneNumber = null
            },
            new Customer
            {
                Id = 2,
                Name = "Andrii",
                Email = "andrii@gmail.com",
                Address = new Address("Kyiv", "Khreshchatyk", "15", "01001"),
                PhoneNumber = null
            },
            new Customer
            {
                Id = 3,
                Name = "Olena",
                Email = "olena@gmail.com",
                Address = new Address("Lviv", "Frankа", "20", "79000"),
                PhoneNumber = null
            },
            new Customer
            {
                Id = 4,
                Name = "Ivan",
                Email = "ivan@gmail.com",
                Address = new Address("Kharkiv", "Sumska", "25", "61000"),
                PhoneNumber = null
            },
            new Customer
            {
                Id = 5,
                Name = "Maria",
                Email = "maria@gmail.com",
                Address = new Address("Odesa", "Derybasivska", "30", "65000"),
                PhoneNumber = null
            }
        ];

        for (int i = 0; i < 20; i++)
        {
            var customer = customers[i % customers.Length];

            var order = new Order(i + 1, customer);

            order.AddItem(
                products[i % products.Length],
                i % 3 + 1);

            order.AddItem(
                products[(i + 2) % products.Length],
                1);

            order.Pay(new CardPayment());

            repository.Add(order);
        }

        return repository;
    }
}
