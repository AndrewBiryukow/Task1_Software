using System;
using System.Threading.Tasks;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp.DAL.Services;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Models
{
    public class LoggedUser
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly UserService userService;

        public LoggedUser(IOrderService orderService, IProductService productService, UserService userService)
        {
            this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task GetUserData(string nickname)
        {
            var user = await userService.GetUserByNicknameAsync(nickname);
            if (user != null)
            {
                Console.WriteLine($"{user.Nickname}, here is your user information:");
                Console.WriteLine($"First Name: {user.FirstName}");
                Console.WriteLine($"Last Name: {user.LastName}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Phone: {user.Phone}");
                Console.WriteLine($"Address: {user.Address}");
                Console.WriteLine($"Gender: {user.Gender}");
                Console.WriteLine($"Bank Card Number: {user.BankCardNumber}");
                Console.WriteLine($"Your Balance: {(user.Balance)/100}$");
                Console.WriteLine($"You have been registered since:{user.CreatedAt}");
                var orders = await orderService.GetUserOrdersByIdAsync(user.Id);
                Console.WriteLine($"Orders for {user.Nickname}:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order ID: {order.Id}, Order Date: {order.OrderDate}, Amount: {order.Amount/100}$, Status: {order.Status}");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public async Task AddOrder(string nickname, int productId, int quantity)
        {
            var user = await userService.GetUserByNicknameAsync(nickname);
            if (user != null)
            {
                var product = await productService.GetProductByIdAsync(productId);

                if (product != null && product.Quantity > 0 && product.Quantity >= quantity)
                {
                    int orderAmount = product.Price * quantity;

                    if (user.Balance >= orderAmount)
                    {
                        product.Quantity -= quantity;
                        user.Balance -= orderAmount;

                        var order = new OrderDTO
                        {
                            UserId = user.Id,
                            ProductId = productId,
                            OrderDate = DateTime.Now,
                            Amount = orderAmount,
                            Status = "Pending",
                            Address = user.Address,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        await productService.UpdateProductAsync(product);
                        await userService.UpdateUserAsync(user);
                        await orderService.AddOrderAsync(order);

                        Console.WriteLine("Order placed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance to place the order.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid product or insufficient quantity available.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public static async Task AddBalance(UserService userService, string nickname, int amount)
        {
            var user = await userService.GetUserByNicknameAsync(nickname);
            if (user != null)
            {
                user.Balance += amount;
                await userService.UpdateUserAsync(user);
                Console.WriteLine($"Balance updated successfully. New balance: {user.Balance}");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public static async Task ReplenishBalance(UserService userService)
        {
            Console.Write("Enter your nickname: ");
            var nickname = Console.ReadLine();

            Console.Write("Enter your bank card number: ");
            var bankCardNumber = Console.ReadLine();

            Console.Write("Enter the amount to replenish your balance: ");
            if (int.TryParse(Console.ReadLine(), out var amount))
            {
                await LoggedUser.AddBalance(userService, nickname, amount);
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a valid numeric value.");
            }
        }
    }
}
