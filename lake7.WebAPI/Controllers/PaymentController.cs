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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
        {
            var newPayment = await _paymentService.ProcessPaymentAsync(payment);
            return Ok(PaymentMapper.ToDto(newPayment));
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments.Select(PaymentMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(Guid id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(PaymentMapper.ToDto(payment));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid id, [FromQuery] string status)
        {
            var updatedPayment = await _paymentService.UpdatePaymentStatusAsync(id, status);
            if (updatedPayment == null) return NotFound();
            return Ok(PaymentMapper.ToDto(updatedPayment));
        }
    }
}
