using System.Collections.Generic;
using System.Threading.Tasks;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int orderId);
        Task<OrderDTO> AddOrderAsync(OrderDTO order);
        Task UpdateOrderAsync(OrderDTO order);
        Task DeleteOrderAsync(int orderId);
        Task<List<OrderDTO>> GetUserOrdersByIdAsync(int userId);
    }
}
