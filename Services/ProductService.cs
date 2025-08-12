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

        public async Task<Product?> CreateProductAsync(CreateProductRequest dto)
        {
            if (string.IsNullOrEmpty(dto.Name) || dto.Price <= 0 || string.IsNullOrEmpty(dto.Category) || string.IsNullOrEmpty(dto.Description))
                return null;
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl
            };
            await _productRepository.CreateProductAsync(product);
            return product;
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteProductAsync(product);
            }
        }
        public async Task<bool> UpdateProductAsync(int id, CreateProductRequest dto)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
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
            await _productRepository.UpdateProductAsync(product);
            return true;
        }
    }
}
