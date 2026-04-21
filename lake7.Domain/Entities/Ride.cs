using lake7.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace lake7.Domain.Entities
{
    public class Ride : CommonEntity
    {
        public Guid UserId { get; set; }
        public Guid? DriverId { get; set; }

        public string PickupLocation { get; set; } = string.Empty;
        public string DropoffLocation { get; set; } = string.Empty;
        public RideStatus Status { get; set; } = RideStatus.Pending;
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropLatitude { get; set; }
        public double DropLongitude { get; set; }
        public DateTime RequestedAt { get; set; }= DateTime.Now;
        public DateTime? CompletedAt { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Driver? Driver { get; set; }

    }
}

