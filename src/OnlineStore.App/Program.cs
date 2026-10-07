using OnlineStore.Domain;

Console.WriteLine("Online Store");

var product1 = new Product(1, "Keyboard", 1200m);
var product2 = new Product(2, "Mouse", 600m);

var customer = new Customer(1, "Taras", "taras@gmail.com");

var order1 = new Order(1, customer);
order1.AddItem(product1, 1);
order1.AddItem(product2, 2);

IPaymentMethod paymentMethod = new CardPayment();
order1.Pay(paymentMethod);

Console.WriteLine($"Order 1 status: {order1.Status}");

Console.WriteLine();

var order2 = new Order(2, customer);
order2.AddItem(product2, 1);

paymentMethod = new BankTransferPayment();
order2.Pay(paymentMethod);

Console.WriteLine($"Order 2 status: {order2.Status}");