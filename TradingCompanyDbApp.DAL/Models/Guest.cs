using System;
using System.Threading.Tasks;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp.DAL.Services;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Models
{
    public class Guest
    {
        private readonly IUserService userService;

        public Guest(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task GetGuestData(string nickname)
        {
            var user = await userService.GetUserByNicknameAsync(nickname);
            if (user != null)
            {
                Console.Clear();
                Console.WriteLine($"{user.Nickname}, here is your user information:\n");
                Console.WriteLine($"First Name: {user.FirstName}");
                Console.WriteLine($"Last Name: {user.LastName}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Phone: {user.Phone}");
                Console.WriteLine($"Address: {user.Address}");
                Console.WriteLine($"Gender: {user.Gender}");
                Console.WriteLine($"Bank Card Number: {user.BankCardNumber}");
                Console.WriteLine($"Your Balance: {(user.Balance) / 100}$");
                Console.WriteLine($"You have been registered since: {user.CreatedAt}");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public async Task Register()
        {
            Console.WriteLine("Register a new user:");

            Console.Write("Enter nickname: ");
            var nickname = Console.ReadLine();

            var existingUser = await userService.GetUserByNicknameAsync(nickname);
            if (existingUser != null)
            {
                Console.WriteLine("Nickname already taken. Choose another one.");
                return;
            }

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            Console.Write("Enter first name: ");
            var firstName = Console.ReadLine();

            Console.Write("Enter last name: ");
            var lastName = Console.ReadLine();

            Console.Write("Enter email: ");
            var email = Console.ReadLine();

            Console.Write("Enter phone: ");
            var phone = Console.ReadLine();

            Console.Write("Enter address: ");
            var address = Console.ReadLine();

            Console.Write("Enter gender: ");
            var gender = Console.ReadLine();

            Console.Write("Enter bank card number: ");
            var bankCardNumber = Console.ReadLine();

            Console.Write("What is your favorite movie(this is your recovery keyword(s)?: ");
            var recoveryKeyword = Console.ReadLine();

            var hashedPassword = Hasher.HashPassword(password);

            var newUser = new UserDTO
            {
                Nickname = nickname,
                Password = hashedPassword,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Address = address,
                Gender = gender,
                BankCardNumber = bankCardNumber,
                RecoveryKeyword = recoveryKeyword,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await userService.CreateUserAsync(newUser);

            Console.WriteLine("User registered successfully.");


        }

        public async Task<bool> Login(string nickname)
        {
            var user = await userService.GetUserByNicknameAsync(nickname);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            Console.Clear();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            if (Hasher.VerifyPassword(password, user.Password))
            {
                Console.WriteLine("Login successful.");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid password.");
                return false;
            }
        }



        public async Task ForgotPassword()
        {
            Console.Write("Enter your nickname: ");
            var nickname = Console.ReadLine();

         
            var user = await userService.GetUserByNicknameAsync(nickname);
            if (user != null)
            {
                Console.Write("Enter the recovery keyword: ");
                var recoveryKeyword = Console.ReadLine();


                if (recoveryKeyword == user.RecoveryKeyword)
                {
                    Console.Write("Enter your new password: ");
                    var newPassword = Console.ReadLine();

                    user.Password = Hasher.HashPassword(newPassword);
                    user.UpdatedAt = DateTime.Now;
                    await userService.UpdateUserAsync(user);
                    Console.WriteLine("Password updated successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid recovery keyword.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
    }
}
