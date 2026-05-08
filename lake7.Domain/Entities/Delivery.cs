using System;
using lake7.Domain.Enums;

namespace lake7.Domain.Entities
{
    public class Delivery : CommonEntity
    {
        public Guid UserId { get; set; }
        public Guid? DriverId { get; set; }

        public string PickupAddress { get; set; } = string.Empty;
        public string DropoffAddress { get; set; } = string.Empty;
        public string PackageDetails { get; set; } = string.Empty;
        public double Fare { get; set; }
        public RideStatus Status { get; set; } = RideStatus.Pending;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public Driver? Driver { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public string SenderName { get; set; } = string.Empty;
        public string SenderPhone { get; set; }= string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; } = string.Empty;
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }
        public string? ItemDescription { get; set; }
    }
}
