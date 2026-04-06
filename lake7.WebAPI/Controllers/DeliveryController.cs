using lake7.Application.DTOs;
using lake7.Application.Helpers;
using lake7.Application.Interface;
using lake7.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [Authorize]
        [HttpPost("request")]
        public async Task<IActionResult> RequestDelivery([FromBody] Delivery delivery)
        {
            var newDelivery = await _deliveryService.RequestDeliveryAsync(delivery);
            return Ok(DeliveryMapper.ToDto(newDelivery));
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync();
            return Ok(deliveries.Select(DeliveryMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryById(Guid id)
        {
            var delivery = await _deliveryService.GetDeliveryByIdAsync(id);
            if (delivery == null) return NotFound();
            return Ok(DeliveryMapper.ToDto(delivery));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateDeliveryStatus(Guid id, [FromQuery] string status)
        {
            var updatedDelivery = await _deliveryService.UpdateDeliveryStatusAsync(id, status);
            if (updatedDelivery == null) return NotFound();
            return Ok(DeliveryMapper.ToDto(updatedDelivery));
        }
    }
}
