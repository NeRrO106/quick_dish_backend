using Microsoft.AspNetCore.Authorization;
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
        private readonly EmailService _emailService;
        private readonly UserService _userService;

        public OrderController(OrderService orderService, EmailService emailService, UserService userService)
        {
            _orderService = orderService;
            _emailService = emailService;
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
            var code = _random.Next(100000, 999999);

            order.DeliveryCode = code;

            await _orderService.CreateOrder(order);


            var user = await _userService.GetUserByIdAsync(order.UserId);

            if (user != null)
            {
                try
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Your order has been placed!",
                        $"<h1>Hello {user.Name}</h1><p>Your order has been placed successfully. Order code: <strong>{code}</strong></p>"
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequiredAdminOrManagerOrCourierRole")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateRequest dto)
        {

            var order = await _orderService.GetOrdersByIdAsync(id);
            if (order == null)
                return NotFound();

            if (dto.Code != null)
            {
                if (dto.Code != order.DeliveryCode)
                    return BadRequest("Invalid code or code expired");
            }

            var updatedOrder = await _orderService.UpdateOrder(id, dto);
            if (!updatedOrder)
                return NotFound();

            var user = await _userService.GetUserByIdAsync(order.UserId);

            if (user != null)
            {
                string message = $"<h1>Hello {user.Name}</h1>";
                bool sendEmail = false;

                if (dto.CourierID.HasValue && dto.CourierID.Value != order.CourierId)
                {
                    var courier = await _userService.GetUserByIdAsync(dto.CourierID.Value);
                    if (courier != null)
                    {
                        message += $"Courier: <strong>{courier.Name}</strong> has picked up your order.<br/>";
                        sendEmail = true;
                    }
                }
                message += "</p>";

                if (sendEmail)
                {
                    try
                    {
                        await _emailService.SendEmailAsync(
                            user.Email,
                            $"Your order #{order.Id} has been updated",
                            message
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            return Ok("Order deleted successfully");
        }
    }
}
