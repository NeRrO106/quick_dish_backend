using Microsoft.EntityFrameworkCore;
using QUickDish.API.Data;
using QUickDish.API.DTOs;
using QUickDish.API.Models;

namespace QUickDish.API.Repos
{
    public class OrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();
        }

        public async Task<Order?> GetOrdersByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByCourierIdAsync(int courierId)
        {
            return await _context.Orders
                .Where(o => o.CourierId == courierId)
                .Include(o => o.Items)
                .ToListAsync();
        }

        public async Task<Order> CreateOrder(Order order)
        {
            if (order == null || order.Items == null || !order.Items.Any())
                throw new ArgumentException("Order must contain at least one item");
            order.CreatedAt = DateTime.Now;
            order.TotalAmount = order.Items.Sum(item => item.TotalPrice);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdateOrder(int id, OrderUpdateRequest dto)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return false;
            if (dto.CourierID.HasValue)
                order.CourierId = dto.CourierID.Value;
            if (!string.IsNullOrEmpty(dto.Status))
                order.Status = dto.Status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

        }
    }
}
