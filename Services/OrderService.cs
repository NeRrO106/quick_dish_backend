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

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        public async Task<Order?> GetOrdersByIdAsync(int id)
        {
            return await _orderRepository.GetOrdersByIdAsync(id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order?> CreateOrder(Order order)
        {
            if (order == null || order.Items == null || !order.Items.Any())
                throw new ArgumentException("Order must contain at least one item");
            order.CreatedAt = DateTime.Now;
            order.TotalAmount = order.Items.Sum(item => item.TotalPrice);

            await _orderRepository.CreateOrder(order);
            return order;
        }
        public async Task<bool> UpdateOrder(int id, OrderUpdateRequest dto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order == null)
                return false;

            if (dto.CourierID.HasValue)
                order.CourierId = dto.CourierID.Value;

            if (!string.IsNullOrEmpty(dto.Status))
                order.Status = dto.Status;

            await _orderRepository.UpdateOrder(order);
            return true;
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order != null)
            {
                await _orderRepository.DeleteOrder(order);
            }
        }
    }
}
