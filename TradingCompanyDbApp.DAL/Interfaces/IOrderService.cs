using System.Collections.Generic;
using System.Threading.Tasks;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int orderId);
        Task<OrderDTO> AddOrderAsync(OrderDTO orderDTO);
        Task UpdateOrderAsync(OrderDTO orderDTO);
        Task DeleteOrderAsync(int orderId);
        Task<List<OrderDTO>> GetUserOrdersByIdAsync(int userId);
    }
}
