using AutoMapper;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp.DTO.ModelsDTO;
using TradingCompanyDbApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace TradingCompanyDbApp.DAL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
                cfg.CreateMap<OrderDTO, Order>()
                    .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                    .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


            });

            _mapper = new Mapper(config);
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> AddOrderAsync(OrderDTO orderDTO)
        {
            OrderDTO order = _mapper.Map<OrderDTO>(orderDTO);
            OrderDTO model = await _orderRepository.AddOrderAsync(order);
            return model;

        }

        public async Task UpdateOrderAsync(OrderDTO orderDTO)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderDTO.Id);

            if (existingOrder == null)
            {
                throw new Exception("Order not found"); 
            }

            
            _mapper.Map(orderDTO, existingOrder);

            await _orderRepository.UpdateOrderAsync(existingOrder);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
        }

        public async Task<List<OrderDTO>> GetUserOrdersByIdAsync(int userId)
        {
            var allOrders = await _orderRepository.GetAllOrdersAsync();
            var userOrders = allOrders.Where(order => order.UserId == userId).ToList();
            return _mapper.Map<List<OrderDTO>>(userOrders);
        }
    }
}
