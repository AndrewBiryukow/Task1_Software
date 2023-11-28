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
using TradingCompanyDbApp.Services;

namespace TradingCompanyDbApp.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        private TradeDbContext dbContext;
        private IProductRepository productRepository;
        private IProductService productService;
        private IMapper mapper;
        public List<int> productsCollector = new List<int>();

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
            productRepository = new ProductRepository(dbContext);
            productService = new ProductService(productRepository);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
                cfg.CreateMap<ProductDTO, ProductDTO>();
            });

            mapper = config.CreateMapper();
        }

        [TestMethod]
        public async Task GetAllProductsAsync_ReturnsListOfProducts()
        {

            // Arrange
            var firstProductDTO = new ProductDTO
            {
                Name = "Product123",
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO firstProductModel = await productService.AddProductAsync(firstProductDTO);
            productsCollector.Add(firstProductModel.Id);

            var secondProductDTO = new ProductDTO
            {
                Name = "Product123",
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO secondProductModel = await productService.AddProductAsync(secondProductDTO);
            productsCollector.Add(secondProductModel.Id);
            // Act
            var result = await productService.GetAllProductsAsync();
            // set count of users
            int expectedCount = 2;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count, "Expected a list that contains " + expectedCount + " products");
        }

        [TestMethod]
        public async Task AddProductAsync_CallsRepositoryWithCorrectProduct()
        {
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

            // Act

            // Assert
            var productInDatabase = await productService.GetProductByIdAsync(productModel.Id);
            Assert.IsNotNull(productInDatabase, "Product not found in the database.");
            Assert.AreEqual(productDTO.Name, productInDatabase.Name, "Product names do not match.");
            // Add more assertions for other properties
        }

        [TestMethod]
        public async Task DeleteProductAsync_DeletesExistingProduct()
        {
            // Arrange
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

            // Act
            var productToDelete = await productRepository.GetProductByIdAsync(productModel.Id);
            Assert.IsNotNull(productToDelete, "Product not found in the database.");

            await productRepository.DeleteProductAsync(productToDelete.Id);

            // Assert
            var deletedProduct = await productRepository.GetProductByIdAsync(productModel.Id);
            Assert.IsNull(deletedProduct, "Product not deleted from the database.");
        }

        [TestMethod]
        public async Task UpdateProductAsync_UpdatesExistingProduct()
        {
            // Arrange
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

            // Add a product to the database

            // Act
            var productToUpdate = await productRepository.GetProductByIdAsync(productModel.Id);
            Assert.IsNotNull(productToUpdate, "Product not found in the database.");

            // Update product properties
            productToUpdate.Name = "UpdatedProduct";
            productToUpdate.Price = 40;
            productToUpdate.Quantity = 90;
            productToUpdate.Description = "Updated product description";
            productToUpdate.UpdatedAt = DateTime.Now;

            await productRepository.UpdateProductAsync(productToUpdate);

            // Assert
            var updatedProduct = await productRepository.GetProductByNameAsync(productToUpdate.Name);
            Assert.IsNotNull(updatedProduct, "Product not found in the database.");

            // Check if product properties are updated
            Assert.AreEqual("UpdatedProduct", updatedProduct.Name, "Product name not updated.");
            Assert.AreEqual(40, updatedProduct.Price, "Product price not updated.");
            Assert.AreEqual(90, updatedProduct.Quantity, "Product quantity not updated.");
            Assert.AreEqual("Updated product description", updatedProduct.Description, "Product description not updated.");
            Assert.AreEqual(productToUpdate.UpdatedAt, updatedProduct.UpdatedAt, "UpdatedAt not updated.");
        }

        [TestMethod]
        public async Task GetProductByNameAsync_ReturnsProductWithMatchingName()
        {
            // Arrange
            var productName = "TestProduct";
            var productDTO = new ProductDTO
            {
                Name = productName,
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO productModel = await productService.AddProductAsync(productDTO);
            productsCollector.Add(productModel.Id);

            // Add a product to the database

            // Act
            var result = await productService.GetProductByIdAsync(productModel.Id);

            // Assert
            Assert.IsNotNull(result, "Product not found.");
            Assert.AreEqual(productDTO.Name, result.Name, "Unexpected product name returned.");
            Assert.AreEqual(productDTO.Price, result.Price, "Unexpected product price returned.");
            Assert.AreEqual(productDTO.Quantity, result.Quantity, "Unexpected quantity name returned.");
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ReturnsProductWithMatchingId()
        {
            // Arrange
            var productDTO = new ProductDTO
            {
                Name = "TestProduct",
                Price = 50,
                Quantity = 100,
                Description = "A test product",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ProductDTO productModel = await productService.AddProductAsync(productDTO);
            productsCollector.Add(productModel.Id);

            // Add a product to the database
            // Get the last product added to the database
            var lastProduct = await productService.GetProductByIdAsync(productModel.Id);

            // Act
            var result = await productService.GetProductByIdAsync(lastProduct.Id);

            // Assert
            Assert.IsNotNull(result, "Product not found.");
            Assert.AreEqual(lastProduct.Id, result.Id, "Unexpected product ID returned.");
            Assert.AreEqual(lastProduct.Name, result.Name, "Unexpected product name returned.");
            Assert.AreEqual(lastProduct.Price, result.Price, "Unexpected product price returned.");
            Assert.AreEqual(lastProduct.Quantity, result.Quantity, "Unexpected product quantity returned.");
            Assert.AreEqual(lastProduct.Description, result.Description, "Unexpected product description returned.");
        }


    public async void Cleaner()
    {
        for (int i = 0; i < productsCollector.Count; i++)
        {
            Console.WriteLine("Cleaned");
            Console.WriteLine(productsCollector[i]);
            await productService.DeleteProductAsync(productsCollector[i]);
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
