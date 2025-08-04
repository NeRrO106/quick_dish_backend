using Microsoft.EntityFrameworkCore;
using QUickDish.API.Data;
using QUickDish.API.DTOs;
using QUickDish.API.Models;

namespace QUickDish.API.Repos
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> GetProductByNameAsync(string name)
        {
            return await _context.Products.AnyAsync(p => p.Name == name);
        }

        public async Task<Product> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateProductAsync(int id, CreateProductDto dto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return false;
            if (!string.IsNullOrEmpty(dto.Name))
                product.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Description))
                product.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.Category))
                product.Category = dto.Category;
            if (dto.Price > 0)
                product.Price = dto.Price;
            if (!string.IsNullOrEmpty(dto.ImageUrl))
                product.ImageUrl = dto.ImageUrl;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
