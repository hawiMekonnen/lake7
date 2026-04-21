using lake7.Domain.Enums;

namespace lake7.Application.DTOs
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid RideId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
    }
}
