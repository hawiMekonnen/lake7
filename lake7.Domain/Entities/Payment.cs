using System;
using lake7.Domain.Enums;

namespace lake7.Domain.Entities
{
    public class Payment: CommonEntity
    {
        public Guid UserId { get; set; }
        public Guid? RideId { get; set; }
        public Guid? DeliveryId { get; set; }
        public User User { get; set; } = null!;
        public Ride? Ride { get; set; }
        public Delivery? Delivery { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
