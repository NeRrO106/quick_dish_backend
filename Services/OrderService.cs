using QUickDish.API.DTOs;
using QUickDish.API.Models;
using QUickDish.API.Repos;

namespace QUickDish.API.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<Orders>> GetOrdersAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        public async Task<Orders?> GetOrdersByIdAsync(int id)
        {
            return await _orderRepository.GetOrdersByIdAsync(id);
        }

        public async Task<List<Orders>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }
        public async Task<List<Orders>> GetOrdersByCourierIdAsync(int courierId)
        {
            return await _orderRepository.GetOrdersByCourierIdAsync(courierId);
        }

        public async Task<Orders?> CreateOrder(Orders order)
        {
            return await _orderRepository.CreateOrder(order);
        }
        public async Task<bool> UpdateOrder(int id, OrderUpdateRequest dto)
        {
            return await _orderRepository.UpdateOrder(id, dto);
        }

        public async Task DeleteOrder(int id)
        {
            await _orderRepository.DeleteOrder(id);
        }
    }
}
