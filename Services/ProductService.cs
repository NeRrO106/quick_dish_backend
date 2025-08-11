using QUickDish.API.DTOs;
using QUickDish.API.Models;
using QUickDish.API.Repos;

namespace QUickDish.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<bool> GetProductByNameAsync(string name)
        {
            return await _productRepository.GetProductByNameAsync(name);
        }

        public async Task<Product> CreateProductAsync(CreateProductRequest dto)
        {
            return await _productRepository.CreateProductAsync(dto);
        }
        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
        public async Task<bool> UpdateProductAsync(int id, CreateProductRequest dto)
        {
            return await _productRepository.UpdateProductAsync(id, dto);
        }
    }
}
