using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Moq;

namespace TradingCompanyDbApp.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private TradeDbContext dbContext;
        private IUserRepository userRepository;
        private IUserService userService;
        private IMapper mapper;
        public List<int> usersCollector = new List<int>();

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
            userRepository = new UserRepository(dbContext);
            userService = new UserService(userRepository);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<UserDTO, UserDTO>();
            });

            mapper = config.CreateMapper();
        }

        [TestMethod]
        public async Task GetAllUsersAsync_ReturnsListOfUsers()
        {


            // Act
            var result = await userService.GetAllUsersAsync();
            // set count of users
            int expectedCount = 21;
            // Assert
            Assert.IsNotNull(result);

            //Assert.AreEqual(expectedCount, result.Count, "Expected a list that contains " + expectedCount + " users");
        }

        [TestMethod]
        public async Task CreateUserAsync_CallsRepositoryWithCorrectUser()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                Nickname = "Johnao14dfej1024as254531",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john10sdj1@example.com",
                Phone = "+1204303431236321321",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"
            };
          
            // Act
            UserDTO model =  await userService.CreateUserAsync(userDTO);
            usersCollector.Add(model.Id);
            
            
            // Assert
            var userInDatabase = await userService.GetUserByNicknameAsync(model.Nickname);
            Assert.IsNotNull(userInDatabase, "User not found in the database."); 
            Assert.AreEqual(model.Nickname, userInDatabase.Nickname, "Nicknames do not match.");

        }

        [TestMethod]
        public async Task GetUserByNicknameAsync_ReturnsUserWithMatchingNickname()
        {

            var existingUser = new UserDTO
            {
                Nickname = "Johno1e2asj1024as254531",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john10s564dj1@example.com",
                Phone = "+1204334312363215647",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"
            };

            // Add a user to the database
            UserDTO model = await userRepository.CreateUserAsync(existingUser);
            usersCollector.Add(model.Id);
            // Act

            var result = await userService.GetUserByNicknameAsync(existingUser.Nickname);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingUser.Nickname, result.Nickname, "Unexpected nickname returned.");
        }


        [TestMethod]
        public async Task DeleteUserAsync_DeletesExistingUser()
        {
            // Arrange
            var existingUser = new UserDTO
            {
                Nickname = "Johno1e2asj1024as254531",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john10s564dj1@example.com",
                Phone = "+1204334312363215647",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"
            };

            // Add a user to the database
            UserDTO model = await userRepository.CreateUserAsync(existingUser);
            usersCollector.Add(model.Id);
            // Act
            var userToDelete = await userRepository.GetUserByNickname(existingUser.Nickname);
            Assert.IsNotNull(userToDelete, "User not found in the database.");

            var userIdToDelete = userToDelete.Id;

            await userRepository.DeleteUserAsync(userIdToDelete);

            // Assert
            var deletedUser = await userRepository.GetUserByIdAsync(userIdToDelete);
            Assert.IsNull(deletedUser, "User not deleted from the database.");
        }

        [TestMethod]
        public async Task UpdateUserAsync_UpdatesExistingUser()
        {
            var existingUser = new UserDTO
            {
                Nickname = "Johno123ej1024as254531",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "john10sdasasdzxcsdj1@example.com",
                Phone = "+120433411131236321",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien2"
            };

            // Add a user to the database
            UserDTO model = await userRepository.CreateUserAsync(existingUser);
            usersCollector.Add(model.Id);
            Console.WriteLine(model.Id);
            // Act
            var userToUpdate = await userRepository.GetUserByNickname(existingUser.Nickname);
            Assert.IsNotNull(userToUpdate, "User not found in the database.");

            // Update user properties
            userToUpdate.FirstName = "UpdatedFirstName";
            userToUpdate.LastName = "UpdatedLastName";
            userToUpdate.Email = "updated.email@example.com";
            userToUpdate.Phone = "+120433431236322";
            userToUpdate.Address = "789 Maple St, Town";
            userToUpdate.Gender = "Female";
            userToUpdate.BankCardNumber = "1234-5678-9012-3456";
            userToUpdate.UpdatedAt = DateTime.Now;

            await userRepository.UpdateUserAsync(userToUpdate);
            Console.WriteLine(userToUpdate.Id);
            // Assert
            var updatedUser = await userRepository.GetUserByIdAsync(userToUpdate.Id);
            Assert.IsNotNull(updatedUser, "User not found in the database.");

            // Check if user properties are updated
            Assert.AreEqual("UpdatedFirstName", updatedUser.FirstName, "First name not updated.");
            Assert.AreEqual("UpdatedLastName", updatedUser.LastName, "Last name not updated.");
            Assert.AreEqual("updated.email@example.com", updatedUser.Email, "Email not updated.");
            Assert.AreEqual("+120433431236322", updatedUser.Phone, "Phone not updated.");
            Assert.AreEqual("789 Maple St, Town", updatedUser.Address, "Address not updated.");
            Assert.AreEqual("Female", updatedUser.Gender, "Gender not updated.");
            Assert.AreEqual("1234-5678-9012-3456", updatedUser.BankCardNumber, "Bank card number not updated.");
            Assert.AreEqual(userToUpdate.UpdatedAt, updatedUser.UpdatedAt, "UpdatedAt not updated.");
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ReturnsUserWithMatchingId()
        {
            // Arrange
            var userInDb = new UserDTO
            {
                Nickname = "TestUser124",
                Password = "asbasd",
                FirstName = "John",
                LastName = "Doe",
                Email = "johnzadbfxc@example.com",
                Phone = "+012043034312321",
                Address = "456 Oak St, City",
                Gender = "Male",
                BankCardNumber = "9876-5432-1098-7654",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RecoveryKeyword = "Alien3"
            };

            // Add a user to the database
            UserDTO model = await userRepository.CreateUserAsync(userInDb);
            usersCollector.Add(model.Id);
            // Get the last user added to the database
            var lastUser = (await userRepository.GetAllUsersAsync()).LastOrDefault();


            // Act
            var result = await userService.GetUserByIdAsync(lastUser.Id);

            // Assert
            Assert.IsNotNull(result, "User not found.");
            Assert.AreEqual(lastUser.Id, result.Id, "Unexpected user ID returned.");
            Assert.AreEqual(lastUser.Nickname, result.Nickname, "Unexpected user nickname returned.");
            Assert.AreEqual(lastUser.FirstName, result.FirstName, "Unexpected user first name returned.");
            Assert.AreEqual(lastUser.LastName, result.LastName, "Unexpected user last name returned.");
            Assert.AreEqual(lastUser.Email, result.Email, "Unexpected user email returned.");
            Assert.AreEqual(lastUser.Phone, result.Phone, "Unexpected user phone returned.");
            Assert.AreEqual(lastUser.Address, result.Address, "Unexpected user address returned.");
            Assert.AreEqual(lastUser.BankCardNumber, result.BankCardNumber, "Unexpected user first bank card number returned.");
            Assert.AreEqual(lastUser.RecoveryKeyword, result.RecoveryKeyword, "Unexpected user recovery keyword returned.");
        }

        public async void Cleaner()
        {

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
