using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QUickDish.API.DTOs;
using QUickDish.API.Services;

namespace QUickDish.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequiredAdminOrManagerOrUserRole")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound("Product not found.");
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Policy = "RequiredAdminOrManagerRole")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest dto)
        {
            if (dto == null)
                return BadRequest("Invalid product data.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Product name cannot be null or empty.");

            if (await _productService.GetProductByNameAsync(dto.Name))
                return BadRequest("A product with this name already exists.");

            var product = await _productService.CreateProductAsync(dto);
            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequiredAdminOrManagerRole")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductRequest dto)
        {
            var product = await _productService.UpdateProductAsync(id, dto);
            if (!product)
                return NotFound("Product not found.");
            return Ok(product);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequiredAdminOrManagerRole")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok("Product deleted successfully.");
        }
    }
}
