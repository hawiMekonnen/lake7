using System;
using System.Collections.Generic;
using System.Text;

namespace lake7.Domain.Entities
{
    public class Delivery
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DriverId { get; set; }
        public string PickupLocation { get; set; } = string.Empty;
        public string DropoffLocation { get; set; } = string.Empty;
        public string PackageDetails { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; 
        public DateTime RequestedAt { get; set; }= DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DeliveredAt { get; set; } = DateTime.Now;
    }
}
