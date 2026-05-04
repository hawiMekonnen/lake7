using lake7.Application.DTOs;
using lake7.Application.Interface;
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
    }
}
