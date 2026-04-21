using lake7.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace lake7.Domain.Entities
{
    public class Delivery : CommonEntity
    {
        public Guid UserId { get; set; }
        public Guid DriverId { get; set; }
        public string PickupLocation { get; set; } = string.Empty;
        public string DropoffLocation { get; set; } = string.Empty;
        public string PackageDetails { get; set; } = string.Empty;
        public RideStatus Status { get; set; }= RideStatus.Pending;
        public DateTime RequestedAt { get; set; }= DateTime.Now;
        public DateTime? DeliveredAt { get; set; } = DateTime.Now;
    }
}
