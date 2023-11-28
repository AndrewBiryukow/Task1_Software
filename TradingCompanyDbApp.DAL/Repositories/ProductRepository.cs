using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingCompanyDbApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DAL.Contexts;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TradeDbContext _dbContext;

        public ProductRepository(TradeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Products.FindAsync(productId);
        }

        public async Task<ProductDTO> AddProductAsync(ProductDTO product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<ProductDTO> GetProductByNameAsync(string productName)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == productName);
        }

        public async Task UpdateProductAsync(ProductDTO product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
