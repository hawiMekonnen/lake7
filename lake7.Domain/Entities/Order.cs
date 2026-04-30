using System;
using lake7.Domain.Enums;

namespace lake7.Domain.Entities
{
    public class Order : CommonEntity
    {
        public Guid UserId { get; set; }
        public Guid? RideId { get; set; }
        public Guid? DeliveryId { get; set; }

        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Navigation
        public User User { get; set; } = null!;
        public Ride? Ride { get; set; }
        public Delivery? Delivery { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
