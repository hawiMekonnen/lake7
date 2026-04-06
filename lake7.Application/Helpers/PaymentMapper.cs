using lake7.Application.DTOs;
using lake7.Domain.Entities;

namespace lake7.Application.Helpers
{
    public static class PaymentMapper
    {
        public static PaymentDto ToDto(Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status
            };
        }
    }
}
