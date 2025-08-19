using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly EmailService _emailService;
        private readonly IMemoryCache _memoryCache;
        private readonly UserService _userService;

        public OrderController(OrderService orderService, EmailService emailService, IMemoryCache memoryCache, UserService userService)
        {
            _orderService = orderService;
            _emailService = emailService;
            _memoryCache = memoryCache;
            _userService = userService;
        }
        [HttpGet]
        [Authorize(Policy = "RequiredAdminOrManagerOrCourierRole")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequiredAdminOrManagerOrCourierRole")]
        public async Task<IActionResult> GetOrdersById(int id)
        {
            var order = await _orderService.GetOrdersByIdAsync(id);
            return Ok(order);
        }

        [HttpGet("orders/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var order = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(order);
        }
        [HttpPost]
        [Authorize(Policy = "RequiredAdminOrManagerOrUserRole")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Invalid order data");

            if (!_orderService.IsValidPhoneNumber(order.PhoneNumber))
                return BadRequest("Invalid phone number");

            Random _random = new Random();
            var code = _random.Next(100000, 999999).ToString();

            await _orderService.CreateOrder(order);

            var user = await _userService.GetUserByIdAsync(order.UserId);

            if (user != null)
            {
                _memoryCache.Set($"order_{order.Id}", code, TimeSpan.FromMinutes(10));
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Comanda ta a fost plasata!",
                    $"<h1>Salut {user.Name}</h1><p>Comanda ta a fost plasata cu succes. Cod comanda: <strong>{code}</strong></p>"
                );

            }
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequiredAdminOrManagerOrCourierRole")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateRequest dto)
        {
            var updatedOrder = await _orderService.UpdateOrder(id, dto);
            if (!updatedOrder)
                return NotFound();

            var order = await _orderService.GetOrdersByIdAsync(id);
            if (order == null)
                return NotFound();

            var user = await _userService.GetUserByIdAsync(order.UserId);

            if (user != null)
            {
                string message = $"<h1>Salut {user.Name}</h1>";
                bool sendEmail = false;

                if (dto.CourierID.HasValue && dto.CourierID.Value != order.CourierId)
                {
                    var courier = await _userService.GetUserByIdAsync(dto.CourierID.Value);
                    if (courier != null)
                    {
                        message += $"Curierul: <strong>{courier.Name}</strong> a preluat comanda ta.<br/>";
                        sendEmail = true;
                    }
                }
                message += "</p>";

                if (sendEmail)
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        $"Comanda ta #{order.Id} a fost actualizată",
                        message
                    );
                }
            }
            if (dto.Code != null)
            {
                if (!_memoryCache.TryGetValue($"order_{order.Id}", out string? cachedCode) || cachedCode != dto.Code.ToString())
                    return BadRequest("Invalid code or code expired");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            return Ok();
        }
    }
}
