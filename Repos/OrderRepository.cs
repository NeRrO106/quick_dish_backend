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

        public async Task<List<OrderResponseDTO>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Select(o => new OrderResponseDTO
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
                    Items = o.Items.Select(i => new OrderItemDTO
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

        public async Task<OrderResponseDTO?> GetOrdersByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.Id == id)
                .Select(o => new OrderResponseDTO
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
                    Items = o.Items.Select(i => new OrderItemDTO
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

        public async Task<List<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderResponseDTO
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
                    Items = o.Items.Select(i => new OrderItemDTO
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
