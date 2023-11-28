using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyDbApp.DAL.Contexts;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DAL.Repositories;
using TradingCompanyDbApp.DAL.Services;

namespace TradingCompanyDbApp.Menu
{
    public class ConsoleMenu
    {
        private readonly Dictionary<string, Func<Task>> _commands;

        public ConsoleMenu()
        {
            _commands = new Dictionary<string, Func<Task>>();
        }

        public void AddCommand(string command, Func<Task> action)
        {
            _commands.Add(command, action);
        }

        public void DisplayMenu(bool isEntryMenu)
        {
            if (isEntryMenu)
            {

                Console.WriteLine("\nWelcome to ACME Trading Company app!");
                Console.WriteLine("\n1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Forgot Password");
                Console.WriteLine("4. Exit\n");
            }
            else
            {
                Console.WriteLine("\n1. Show your user information");
                Console.WriteLine("2. Sign out");
                Console.WriteLine("3. Exit\n");
            }
        }

        public async Task ExecuteCommand(string command)
        {
            if (_commands.TryGetValue(command, out var action))
            {
                try
                {
                    await action.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing command: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }

        public static async Task EntryMenu(TradeDbContext tradeDbContext)
        {
            var menu = new ConsoleMenu();
            var guest = new Guest(new UserService(new UserRepository(tradeDbContext)));
            menu.AddCommand("1", async () => await RegistrationMenu(guest,tradeDbContext));
            menu.AddCommand("2", async () => await LoginMenu(guest, menu, tradeDbContext));
            menu.AddCommand("3", async () => await guest.ForgotPassword());
            menu.AddCommand("4", () => Task.CompletedTask);

            int choice;

            do
            {

                menu.DisplayMenu(true);
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    await menu.ExecuteCommand(choice.ToString());
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input. Please enter a correspond option from menu.");
                }
            } while (choice != 4);

            await menu.ExecuteCommand(choice.ToString());
        }

        public static async Task LoginMenu(Guest guest, ConsoleMenu entryMenu, TradeDbContext tradeDbContext)
        {
            var loginMenu = new ConsoleMenu();

            Console.WriteLine("\nLogin:");
            Console.Write("Enter your nickname: ");
            string nickname = Console.ReadLine();
            bool loginSuccessful = await guest.Login(nickname);

            if (loginSuccessful)
            {
                Console.Clear();
                Console.WriteLine("\nLogin successful.\n");
                loginMenu.AddCommand("1", async () => await guest.GetGuestData(nickname));
                loginMenu.AddCommand("2", async () => await SigningOut(tradeDbContext));
                loginMenu.AddCommand("3", () => Task.CompletedTask);

                int choice;
                do
                {
                    loginMenu.DisplayMenu(false);
                    Console.Write("Enter your choice: ");

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        await loginMenu.ExecuteCommand(choice.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a correspond option from menu.");
                    }
                } while (choice != 3 && choice != 2);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Login failed - there's no user with such credentials. Returning to the main menu.");
                await EntryMenu(tradeDbContext);
            }
        }

        public static async Task RegistrationMenu(Guest guest, TradeDbContext tradeDbContext)
        {
            await guest.Register();
            await EntryMenu(tradeDbContext);
        }

        public static async Task SigningOut(TradeDbContext tradeDbContext)
        {
            Console.Clear();
            Console.WriteLine("Signing out...");
            await EntryMenu(tradeDbContext);
        }
    }

}
