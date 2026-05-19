using lake7.Application.DTOs;
using lake7.Application.Interface;
using lake7.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost("place-delivery")]
        public async Task<IActionResult> PlaceDeliveryOrder([FromBody] PlaceDeliveryOrderDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid User ID");
            }

            try
            {
                var order = await _orderService.PlaceDeliveryOrderAsync(userId, dto);
                return Ok(new 
                {
                    id = order.Id,
                    deliveryId = order.DeliveryId,
                    status = order.Status.ToString(),
                    totalAmount = order.TotalAmount,
                    createdAt = order.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] OrderStatus? status)
        {
            if (status.HasValue)
            {
                var orders = await _orderService.GetOrdersByStatusAsync(status.Value);
                return Ok(orders);
            }
            else
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
        }

        [HttpPatch("{id}/assign/{driverId}")]
        public async Task<IActionResult> AssignDriver(Guid id, Guid driverId)
        {
            var order = await _orderService.AssignDriverAsync(id, driverId);
            if (order == null) return NotFound("Order not found");
            return Ok(order);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] OrderStatus status)
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, status);
            if (order == null) return NotFound("Order not found");
            return Ok(order);
        }

        [HttpGet("driver/{driverId}/active")]
        public async Task<IActionResult> GetActiveOrderByDriverId(Guid driverId)
        {
            var order = await _orderService.GetActiveOrderByDriverIdAsync(driverId);
            if (order == null) return NotFound("No active order found for this driver");
            return Ok(order);
        }
    }
}

