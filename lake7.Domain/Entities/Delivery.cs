using System;
using lake7.Domain.Enums;

namespace lake7.Domain.Entities
{
    public class Delivery : CommonEntity
    {
        public Guid UserId { get; set; }
        public Guid DriverId { get; set; }

        public string PickupLocation { get; set; } = string.Empty;
        public string DropoffLocation { get; set; } = string.Empty;
        public string PackageDetails { get; set; } = string.Empty;
        public RideStatus Status { get; set; } = RideStatus.Pending;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public Driver Driver { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
