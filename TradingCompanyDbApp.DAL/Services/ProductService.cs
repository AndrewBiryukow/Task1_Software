using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DAL.Repositories;
using TradingCompanyDbApp.DTO.ModelsDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TradingCompanyDbApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDTO, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
                cfg.CreateMap<Product, ProductDTO>();
            });

            _mapper = new Mapper(config);
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> GetProductByNameAsync(string productName)
        {
            var product = await _productRepository.GetProductByNameAsync(productName);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> AddProductAsync(ProductDTO productDto)
        {
            ProductDTO product = _mapper.Map<ProductDTO>(productDto);
            ProductDTO model = await _productRepository.AddProductAsync(product);
            return model;
        }

        public async Task UpdateProductAsync(ProductDTO productDto)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(productDto.Id);
            if (existingProduct == null)
            {

                throw new Exception("Product not found");
            }

            existingProduct.Name = productDto.Name;
            existingProduct.Price = productDto.Price;
            existingProduct.Quantity = productDto.Quantity;
            existingProduct.Description = productDto.Description;
            existingProduct.UpdatedAt = DateTime.Now;

            await _productRepository.UpdateProductAsync(existingProduct);
        }

        public async Task DeleteProductAsync(int productId)
        {
            await _productRepository.DeleteProductAsync(productId);
        }
    }
}
