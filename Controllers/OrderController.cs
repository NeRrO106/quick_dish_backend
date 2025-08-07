using Microsoft.AspNetCore.Mvc;
using QUickDish.API.DTOs;
using QUickDish.API.Models;
using QUickDish.API.Services;

namespace QUickDish.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdersById(int id)
        {
            var order = await _orderService.GetOrdersByIdAsync(id);
            return Ok(order);
        }

        [HttpGet("/orders/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var order = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(order);
        }
        [HttpGet("/courier/{courierId}")]
        public async Task<IActionResult> GetOrdersByCourierId(int courierId)
        {
            var order = await _orderService.GetOrdersByCourierIdAsync(courierId);
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Invalid order data");
            var user = await _orderService.CreateOrder(order);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateRequest dto)
        {
            var order = await _orderService.UpdateOrder(id, dto);
            if (!order)
                return NotFound();
            return Ok(order);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            return Ok();
        }
    }
}
