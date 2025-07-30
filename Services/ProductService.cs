using QUickDish.API.DTOs;
using QUickDish.API.Models;
using QUickDish.API.Repos;

namespace QUickDish.API.Services
{
    public class ProductService
    {
        private readonly ProductRepo _productRepo;
        public ProductService(ProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepo.GetAllProductsAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepo.GetProductByIdAsync(id);
        }

        public async Task<bool> GetProductByNameAsync(string name)
        {
            return await _productRepo.GetProductByNameAsync(name);
        }

        public async Task<Product> CreateProductAsync(CreateProductDto dto)
        {
            return await _productRepo.CreateProductAsync(dto);
        }
        public async Task DeleteProductAsync(int id)
        {
            await _productRepo.DeleteProductAsync(id);
        }
        public async Task<bool> UpdateProductAsync(int id, CreateProductDto dto)
        {
            return await _productRepo.UpdateProductAsync(id, dto);
        }
    }
}
