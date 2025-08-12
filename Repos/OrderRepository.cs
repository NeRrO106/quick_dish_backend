using Microsoft.EntityFrameworkCore;
using QUickDish.API.Data;
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
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();

            foreach (var order in orders)
            {
                order.UserName = _context.Users
                    .Where(u => u.Id == order.UserId)
                    .Select(u => u.Name)
                    .FirstOrDefault() ?? string.Empty;
                order.CourierName = _context.Users
                    .Where(u => u.Id == order.CourierId)
                    .Select(u => u.Name)
                    .FirstOrDefault() ?? string.Empty;
            }
            foreach (var item in orders.SelectMany(o => o.Items))
            {
                item.ProductName = _context.Products
                    .Where(p => p.Id == item.ProductId)
                    .Select(p => p.Name)
                    .FirstOrDefault() ?? string.Empty;
                item.ProductDescription = _context.Products
                    .Where(p => p.Id == item.ProductId)
                    .Select(p => p.Description)
                    .FirstOrDefault() ?? string.Empty;
            }
            return orders;
        }

        public async Task<Order?> GetOrdersByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();

            if (order != null)
            {
                order.UserName = _context.Users
                    .Where(u => u.Id == order.UserId)
                    .Select(u => u.Name)
                    .FirstOrDefault() ?? string.Empty;

                order.CourierName = _context.Users
                    .Where(u => u.Id == order.CourierId)
                    .Select(u => u.Name)
                    .FirstOrDefault() ?? string.Empty;
                foreach (var item in order.Items)
                {
                    item.ProductName = _context.Products
                        .Where(p => p.Id == item.ProductId)
                        .Select(p => p.Name)
                        .FirstOrDefault() ?? string.Empty;
                    item.ProductDescription = _context.Products
                        .Where(p => p.Id == item.ProductId)
                        .Select(p => p.Description)
                        .FirstOrDefault() ?? string.Empty;
                }
            }
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            foreach (var order in orders)
            {
                order.UserName = _context.Users
                    .Where(u => u.Id == order.UserId)
                    .Select(u => u.Name)
                    .FirstOrDefault() ?? string.Empty;

                order.CourierName = _context.Users
                    .Where(u => u.Id == order.CourierId)
                    .Select(u => u.Name)
                    .FirstOrDefault() ?? string.Empty;
            }
            foreach (var item in orders.SelectMany(o => o.Items))
            {
                item.ProductName = _context.Products
                    .Where(p => p.Id == item.ProductId)
                    .Select(p => p.Name)
                    .FirstOrDefault() ?? string.Empty;
                item.ProductDescription = _context.Products
                    .Where(p => p.Id == item.ProductId)
                    .Select(p => p.Description)
                    .FirstOrDefault() ?? string.Empty;
            }
            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
