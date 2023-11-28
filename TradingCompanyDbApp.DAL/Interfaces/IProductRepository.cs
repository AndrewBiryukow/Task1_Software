using TradingCompanyDbApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int productId);
        Task<ProductDTO> AddProductAsync(ProductDTO product);
        Task UpdateProductAsync(ProductDTO product);

        Task<ProductDTO> GetProductByNameAsync(string productName);
        Task DeleteProductAsync(int productId);


    }
}
