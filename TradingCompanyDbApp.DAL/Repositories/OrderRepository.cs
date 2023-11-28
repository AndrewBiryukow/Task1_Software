using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingCompanyDbApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DAL.Contexts;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.TradeApp.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TradeDbContext _dbContext;

        public OrderRepository(TradeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders.FindAsync(orderId);
        }

        public async Task<OrderDTO> AddOrderAsync(OrderDTO order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderAsync(OrderDTO order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<OrderDTO>> GetUserOrdersByIdAsync(int userId)
        {
            return await _dbContext.Orders.Where(order => order.UserId == userId).ToListAsync();
        }

    }
}
