using Microsoft.EntityFrameworkCore;
using TradingCompanyDbApp.DAL;
using TradingCompanyDbApp.DAL.Contexts;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp.DAL.Repositories;
using TradingCompanyDbApp.DTO;
using TradingCompanyDbApp.DTO.ModelsDTO;
using TradingCompanyDbApp.Menu;
using TradingCompanyDbApp.Services;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using TradingCompanyDbApp.TradeApp.DAL.Repositories;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using TradingCompanyDbApp.DAL.Services;

class TradingCompanyDbConsoleApp
{
    private static IUserService userService;
    private static IOrderService orderService;
    private static IProductService productService;
    private static LoggedUser loggedUser;
    private static TradeDbContext dbContext;

    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../../"))
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("TradeDbConnection");

        var optionsBuilder = new DbContextOptionsBuilder<TradeDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        dbContext = new TradeDbContext(optionsBuilder.Options);

        userService = new UserService(new UserRepository(dbContext));
        orderService = new OrderService(new OrderRepository(dbContext));
        productService = new ProductService(new ProductRepository(dbContext));

        await ConsoleMenu.EntryMenu(dbContext);
    }

    


}


/*
private static void AddProduct()
{
    Console.WriteLine("Add a new product:");

    Console.Write("Enter product name: ");
    var productName = Console.ReadLine();

    Console.Write("Enter price: ");
    if (int.TryParse(Console.ReadLine(), out var price))
    {
        Console.Write("Enter quantity: ");
        if (int.TryParse(Console.ReadLine(), out var quantity))
        {
            Console.Write("Enter description: ");
            var description = Console.ReadLine();

            var newProduct = new ProductDTO
            {
                Name = productName,
                Price = price,
                Quantity = quantity,
                Description = description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            productService.AddProductAsync(newProduct).Wait();

            Console.WriteLine("Product added successfully.");
        }
        else
        {
            Console.WriteLine("Invalid quantity. Please enter a valid numeric value.");
        }
    }
    else
    {
        Console.WriteLine("Invalid price. Please enter a valid numeric value.");
    }
}


private static void AddOrder()
{
    Console.WriteLine("Add a new order:");

    Console.Write("Enter nickname: ");
    var nickname = Console.ReadLine();

    Console.Write("Enter product ID: ");
    if (int.TryParse(Console.ReadLine(), out var productId))
    {
        Console.Write("Enter quantity: ");
        if (int.TryParse(Console.ReadLine(), out var quantity))
        {
            Console.Write("Enter address: ");
            var address = Console.ReadLine();

            var user = userService.GetUserByNicknameAsync(nickname).Result;

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }


            var product = productService.GetProductByIdAsync(productId).Result;

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }


            var newOrder = new OrderDTO
            {
                UserId = user.Id,
                ProductId = product.Id,
                OrderDate = DateTime.Now,
                Amount = quantity * product.Price,
                Address = address,
                Status = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            orderService.AddOrderAsync(newOrder).Wait();

            Console.WriteLine("Order placed successfully.");
        }
        else
        {
            Console.WriteLine("Invalid quantity. Please enter a valid numeric value.");
        }
    }
    else
    {
        Console.WriteLine("Invalid product ID. Please enter a valid numeric value.");
    }
}


private static void AddBalance()
{
    Console.WriteLine("Add balance to the user:");

    Console.Write("Enter nickname: ");
    var nickname = Console.ReadLine();

    Console.Write("Enter amount: ");
    if (int.TryParse(Console.ReadLine(), out var amount))
    {
        LoggedUser.AddBalance(new UserService(new UserRepository(dbContext)), nickname, amount).Wait();
    }
    else
    {
        Console.WriteLine("Invalid amount. Please enter a valid numeric value.");
    }
}
}
*/

/*
private static async Task Main(string[] args)
{
    Console.WriteLine("Trading Company Console App");

    var optionsBuilder = new DbContextOptionsBuilder<TradeDbContext>();
    optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=TradingCompanyDB;Trusted_Connection=True;TrustServerCertificate=True;");

    using (var dbContext = new TradeDbContext(optionsBuilder.Options))
    {
        IUserRepository userRepository = new UserRepository(dbContext);
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);

        var userService = new UserService(userRepository);
        var productService = new ProductService(productRepository);
        var orderService = new OrderService(orderRepository);

        await DisplayAllUsersAsync(userService);
        await DisplayAllProducts(productService);
        await DisplayAllOrders(orderService);

        var newUserDTO = new UserDTO
        {
            Nickname = "Johnoej11024as254531",
            Password="asbasd",
            FirstName = "John",
            LastName = "Doe",
            Email = "john101sdj1242@example.com",
            Phone = "+12043312363",
            Address = "456 Oak St, City",
            Gender = "Male",
            BankCardNumber = "9876-5432-1098-7654"
        };
        await userService.CreateUserAsync(newUserDTO);

        var newProductDTO = new ProductDTO
        {
            Name = "New Product",
            Price = 19.99m,
            Quantity = 50,
            Description = "A new product description"
        };
        await productService.AddProductAsync(newProductDTO);

        var newOrderDTO = new OrderDTO
        {
            UserId = newUserDTO.Id,
            ProductId = newProductDTO.Id,
            OrderDate = DateTime.Now,
            Amount = 2,
            Address = "456 Oak St, City",
            Status = "Pending"
        };
        await orderService.AddOrderAsync(newOrderDTO);

        await DisplayAllUsersAsync(userService);
        await DisplayAllProducts(productService);
        await DisplayAllOrders(orderService);
    }
}

private static async Task DisplayAllUsersAsync(UserService userService)
{
    var users = await userService.GetAllUsersAsync();
    Console.WriteLine("\nAll Users:");
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Id}: {user.Nickname} - {user.Email}");
    }
    Console.WriteLine();
}

private static async Task DisplayAllProducts(ProductService productService)
{
    var products = await productService.GetAllProductsAsync();
    Console.WriteLine("\nAll Products:");
    foreach (var product in products)
    {
        Console.WriteLine($"{product.Id}: {product.Name} - {product.Price:C}");
    }
    Console.WriteLine();
}

private static async Task DisplayAllOrders(OrderService orderService)
{
    var orders = await orderService.GetAllOrdersAsync();
    Console.WriteLine("\nAll Orders:");
    foreach (var order in orders)
    {
        var userNickname = order.User != null ? order.User.Nickname : "Unknown User";
        var productName = order.Product != null ? order.Product.Name : "Unknown Product";
        Console.WriteLine($"{order.Id}: User - {userNickname}, Product - {productName}, Amount - {order.Amount}");
    }
    Console.WriteLine();
}
}



    var optionsBuilder = new DbContextOptionsBuilder<TradeDbContext>();
    optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=TradingCompanyDB;Trusted_Connection=True;TrustServerCertificate=True;");

    using (TradeDbContext db = new TradeDbContext(optionsBuilder.Options))
    {

        User tom = new User
        {
            Nickname = "Tomas21s",
            Password = "somePassword",
            FirstName = "Tom",
            LastName = "LastName",
            Email = "tomas21s@example.com",
            Phone = "+123456789012",
            Address = "123 Main St, City",
            Gender = "Male",
            BankCardNumber = "1234567890123456"
        };


        db.Users.Add(tom);
        db.SaveChanges();
        Console.WriteLine("Objects has been saved");

        var users = db.Users.ToList();
        Console.WriteLine("Objects list:");
        foreach (User u in users)
        {
            Console.WriteLine($"{u.Id}.{u.Nickname} - {u.Password} - {u.FirstName} - {u.LastName} - {u.Email} - {u.Phone} - {u.Address} - {u.Gender} - {u.BankCardNumber}");
        }
    }
    */