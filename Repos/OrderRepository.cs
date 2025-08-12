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

        public async Task<List<OrderResponse>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Select(o => new OrderResponse
                {
                    Id = o.Id,
                    UserName = _context.Users
                        .Where(u => u.Id == o.UserId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                    CourierName = _context.Users
                        .Where(u => u.Id == o.CourierId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                    Address = o.Address,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    Items = o.Items.Select(i => new OrderItemRequest
                    {
                        ProductName = _context.Products
                        .Where(p => p.Id == i.ProductId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                        ProductDescription = _context.Products
                        .Where(p => p.Id == i.ProductId)
                        .Select(u => u.Description)
                        .FirstOrDefault(),
                        Quantity = i.Quantity,
                        Price = i.UnitPrice,
                        TotalPrice = i.TotalPrice,
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<OrderResponse?> GetOrdersByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.Id == id)
                .Select(o => new OrderResponse
                {
                    Id = o.Id,
                    UserName = _context.Users
                        .Where(u => u.Id == o.UserId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                    CourierName = _context.Users
                        .Where(u => u.Id == o.CourierId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                    Address = o.Address,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    Items = o.Items.Select(i => new OrderItemRequest
                    {
                        ProductName = _context.Products
                        .Where(p => p.Id == i.ProductId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                        ProductDescription = _context.Products
                        .Where(p => p.Id == i.ProductId)
                        .Select(u => u.Description)
                        .FirstOrDefault(),
                        Quantity = i.Quantity,
                        Price = i.UnitPrice,
                        TotalPrice = i.TotalPrice,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderResponse>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderResponse
                {
                    Id = o.Id,
                    UserName = _context.Users
                        .Where(u => u.Id == o.UserId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                    CourierName = _context.Users
                        .Where(u => u.Id == o.CourierId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                    Address = o.Address,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    Items = o.Items.Select(i => new OrderItemRequest
                    {
                        ProductName = _context.Products
                        .Where(p => p.Id == i.ProductId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                        ProductDescription = _context.Products
                        .Where(p => p.Id == i.ProductId)
                        .Select(u => u.Description)
                        .FirstOrDefault(),
                        Quantity = i.Quantity,
                        Price = i.UnitPrice,
                        TotalPrice = i.TotalPrice,
                    }).ToList()
                })
                .ToListAsync();
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
