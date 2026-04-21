using lake7.Application.DTOs;
using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto dto)
        {
            var payment = new Payment
            {
                RideId = dto.RideId,
                Amount = dto.Amount,
                Method = dto.Method,
                Status = dto.Status
            };

            var newPayment = await _paymentService.CreatePaymentAsync(payment);
            return Ok(newPayment);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(Guid id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid id, [FromQuery] PaymentStatus status)
        {
            var updatedPayment = await _paymentService.UpdatePaymentStatusAsync(id, status);
            if (updatedPayment == null) return NotFound();
            return Ok(updatedPayment);
        }
    }
}
