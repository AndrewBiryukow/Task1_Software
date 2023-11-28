using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingCompanyDbApp.DAL.Contexts;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DAL.Repositories;
using TradingCompanyDbApp.DAL.Services;
using TradingCompanyDbApp.DTO.ModelsDTO;
using TradingCompanyDbApp.Services;
using TradingCompanyDbApp.TradeApp.DAL.Repositories;

namespace TradingCompanyDbApp.Tests
{
    [TestClass]
    public class OrderRepositoryTests
    {

        private TradeDbContext dbContext;
        private IOrderRepository orderRepository;
        private IOrderService orderService;
        private IUserService userService;
        private IProductService productService;
        private IUserRepository userRepository;
        private IProductRepository productRepository;
        private IMapper mapper;
        public List<int> usersCollector = new List<int>();
        public List<int> productsCollector = new List<int>();
        public List<int> ordersCollector = new List<int>();

        [TestInitialize]
        public void TestInitialize()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../../"))
                .AddJsonFile("testappsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("TradeTestDbConnection");

            var optionsBuilder = new DbContextOptionsBuilder<TradeDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            dbContext = new TradeDbContext(optionsBuilder.Options);
            orderRepository = new OrderRepository(dbContext);
            productRepository = new ProductRepository(dbContext);
            userRepository = new UserRepository(dbContext);
            userService = new UserService(userRepository);
            orderService = new OrderService(orderRepository);
            productService = new ProductService(productRepository);


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>()
                    .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                    .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
                cfg.CreateMap<OrderDTO, Order>()
                    .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                    .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            });

            mapper = config.CreateMapper();
        }

        [TestMethod]
        public async Task GetAllOrdersAsync_ReturnsListOfOrders()
        {
            // Arrange
            var userDTO = new UserDTO()
            {
                Nickname = "John0",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john0@example.com",
                Phone = "+120431236321",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"

            };

            UserDTO userModel = await userService.CreateUserAsync(userDTO);
            usersCollector.Add(userModel.Id);

            var productDTO = new ProductDTO
            {
                Name = "Product123",
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO productModel = await productService.AddProductAsync(productDTO);
            productsCollector.Add(productModel.Id);

            // Arrange
            var firstOrderDTO = new OrderDTO
            {
                UserId = userModel.Id,
                ProductId = productModel.Id,
                OrderDate = DateTime.Now,
                Amount = 2,
                Address = "123 Main St",
                Status = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            var secondOrderDTO = new OrderDTO
            {
                UserId = userModel.Id,
                ProductId = productModel.Id,
                OrderDate = DateTime.Now,
                Amount = 2,
                Address = "123 Main St",
                Status = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            OrderDTO orderFirstModel = await orderService.AddOrderAsync(firstOrderDTO);
            OrderDTO orderSecondModel = await orderService.AddOrderAsync(secondOrderDTO);
            ordersCollector.Add(orderFirstModel.Id);
            ordersCollector.Add(orderSecondModel.Id);

            // Act
            var result = await orderService.GetAllOrdersAsync();
            // set count of orders
            int expectedCount = 2;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count, "Expected a list that contains " + expectedCount + " orders");
        }

        [TestMethod]
        public async Task AddOrderAsync_CallsRepositoryWithCorrectOrder()
        {
            var userDTO = new UserDTO()
            {
                Nickname = "John4",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john4@example.com",
                Phone = "+120431232131",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"

            };

            UserDTO userModel = await userService.CreateUserAsync(userDTO);
            usersCollector.Add(userModel.Id);
            Console.WriteLine(userModel.Id.ToString());
            var productDTO = new ProductDTO
            {
                Name = "Product123",
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO productModel = await productService.AddProductAsync(productDTO);
            productsCollector.Add(productModel.Id);

            // Arrange
            var orderDTO = new OrderDTO
            {
                UserId = userModel.Id,
                ProductId = productModel.Id,
                OrderDate = DateTime.Now,
                Amount = 2,
                Address = "123 Main St",
                Status = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            OrderDTO orderModel = await orderService.AddOrderAsync(orderDTO);
            ordersCollector.Add(orderModel.Id);


            // Act

            // Assert
            var orderInDatabase = await orderService.GetOrderByIdAsync(orderDTO.Id);
            Assert.IsNotNull(orderInDatabase, "Order not found in the database.");
            Assert.AreEqual(orderDTO.UserId, orderInDatabase.UserId, "User IDs do not match.");
            Assert.AreEqual(orderDTO.ProductId, orderInDatabase.ProductId, "Product IDs do not match.");
        }

        [TestMethod]
        public async Task DeleteOrderAsync_DeletesExistingOrder()
        {
            // Arrange
            var userDTO = new UserDTO()
            {
                Nickname = "John1",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john12@example.com",
                Phone = "+102043321",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"

            };

            UserDTO userModel = await userService.CreateUserAsync(userDTO);
            usersCollector.Add(userModel.Id);

            var productDTO = new ProductDTO
            {
                Name = "Product123",
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO productModel = await productService.AddProductAsync(productDTO);
            productsCollector.Add(productModel.Id);

            // Arrange
            var orderDTO = new OrderDTO
            {
                UserId = userModel.Id,
                ProductId = productModel.Id,
                OrderDate = DateTime.Now,
                Amount = 2,
                Address = "123 Main St",
                Status = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            OrderDTO orderModel = await orderService.AddOrderAsync(orderDTO);
            ordersCollector.Add(orderModel.Id);


            // Act
            var orderToDelete = await orderRepository.GetOrderByIdAsync(orderDTO.Id);
            Assert.IsNotNull(orderToDelete, "Order not found in the database.");

            var orderIdToDelete = orderToDelete.Id;

            await orderRepository.DeleteOrderAsync(orderIdToDelete);

            // Assert
            var deletedOrder = await orderRepository.GetOrderByIdAsync(orderIdToDelete);
            Assert.IsNull(deletedOrder, "Order not deleted from the database.");
        }

        [TestMethod]
        public async Task UpdateOrderAsync_UpdatesExistingOrder()
        {
            // Arrange
            var userDTO = new UserDTO()
            {
                Nickname = "John2",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john2@example.com",
                Phone = "+10230321321",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"

            };

            UserDTO userModel = await userService.CreateUserAsync(userDTO);
            usersCollector.Add(userModel.Id);

            var productDTO = new ProductDTO
            {
                Name = "Product123",
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO productModel = await productService.AddProductAsync(productDTO);
            productsCollector.Add(productModel.Id);

            // Arrange
            var orderDTO = new OrderDTO
            {
                UserId = userModel.Id,
                ProductId = productModel.Id,
                OrderDate = DateTime.Now,
                Amount = 2,
                Address = "123 Main St",
                Status = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                User = userModel,
                Product = productModel
            };

            OrderDTO orderModel = await orderService.AddOrderAsync(orderDTO);
            ordersCollector.Add(orderModel.Id);
            // Add an order to the database

            // Act
            var orderToUpdate = await orderRepository.GetOrderByIdAsync(orderDTO.Id);
            Assert.IsNotNull(orderToUpdate, "Order not found in the database.");

            // Updating order properties
            orderToUpdate.Amount = 5;
            orderToUpdate.Address = "456 Oak St";
            orderToUpdate.Status = "Shipped";
            orderToUpdate.UpdatedAt = DateTime.Now;

            await orderRepository.UpdateOrderAsync(orderToUpdate);

            
            var updatedOrder = await orderRepository.GetOrderByIdAsync(orderToUpdate.Id);
            Assert.IsNotNull(updatedOrder, "Order not found in the database.");

            Assert.AreEqual(5, updatedOrder.Amount, "Order amount not updated.");
            Assert.AreEqual("456 Oak St", updatedOrder.Address, "Order address not updated.");
            Assert.AreEqual("Shipped", updatedOrder.Status, "Order status not updated.");
            Assert.AreEqual(orderToUpdate.UpdatedAt, updatedOrder.UpdatedAt, "UpdatedAt not updated.");
        }

        [TestMethod]
        public async Task GetOrderByIdAsync_ReturnsOrderWithMatchingId()
        {
            var userDTO = new UserDTO()
            {
                Nickname = "John3",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john3@example.com",
                Phone = "+112330430321",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"

            };

            UserDTO userModel = await userService.CreateUserAsync(userDTO);
            usersCollector.Add(userModel.Id);

            var productDTO = new ProductDTO
            {
                Name = "Product123",
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO productModel = await productService.AddProductAsync(productDTO);
            productsCollector.Add(productModel.Id);

            // Arrange
            var orderDTO = new OrderDTO
            {
                UserId = userModel.Id,
                ProductId = productModel.Id,
                OrderDate = DateTime.Now,
                Amount = 2,
                Address = "123 Main St",
                Status = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                User = userModel,
                Product = productModel
            };

            OrderDTO orderModel = await orderService.AddOrderAsync(orderDTO);
            ordersCollector.Add(orderModel.Id);

            // Add an order to the database

            // Get the last order added to the database
            var lastOrder = (await orderRepository.GetAllOrdersAsync()).LastOrDefault();


            // Act
            var result = await orderService.GetOrderByIdAsync(orderModel.Id);

            // Assert
            Assert.IsNotNull(result, "Order not found.");
            Assert.AreEqual(lastOrder.Id, result.Id, "Unexpected order ID returned.");
            Assert.AreEqual(lastOrder.UserId, result.UserId, "Unexpected user ID returned.");

        }


        [TestMethod]
        public async Task GetUserOrdersByIdAsync_ReturnsListOfOrdersForUser()
        {
            var userDTO = new UserDTO()
            {
                Nickname = "John3",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john3@example.com",
                Phone = "+112330430321",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"

            };

            UserDTO userModel = await userService.CreateUserAsync(userDTO);
            usersCollector.Add(userModel.Id);

            var productDTO = new ProductDTO
            {
                Name = "Product123",
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO productModel = await productService.AddProductAsync(productDTO);
            productsCollector.Add(productModel.Id);


            var orderDTO = new OrderDTO
            {
                UserId = userModel.Id,
                ProductId = productModel.Id,
                OrderDate = DateTime.Now,
                Amount = 2,
                Address = "123 Main St",
                Status = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                User = userModel,
                Product = productModel
            };

            OrderDTO orderModel = await orderService.AddOrderAsync(orderDTO);
            ordersCollector.Add(orderModel.Id);
            // Arrange
            //var orderService = new OrderService(orderRepository);

            // Act
            var result = await orderService.GetUserOrdersByIdAsync(userModel.Id);
            // set count of orders for the specific user
            int expectedCount = 1;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count, "Expected a list that contains " + expectedCount + " orders for the user with ID " + userModel.Id);
        }

        public async void Cleaner()
        {
            for (int i = 0; i < ordersCollector.Count; i++)
            {
                Console.WriteLine("Cleaned");
                Console.WriteLine(ordersCollector[i]);
                await orderService.DeleteOrderAsync(ordersCollector[i]);
            }
            for (int i = 0; i < productsCollector.Count; i++)
            {
                Console.WriteLine("Cleaned");
                Console.WriteLine(productsCollector[i]);
                await productService.DeleteProductAsync(productsCollector[i]);
            }
            for (int i = 0; i < usersCollector.Count; i++)
            {
                Console.WriteLine("Cleaned");
                Console.WriteLine(usersCollector[i]);
                await userService.DeleteUserAsync(usersCollector[i]);
            }
            dbContext.Dispose();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Cleaner();
        }

        

    }
}
