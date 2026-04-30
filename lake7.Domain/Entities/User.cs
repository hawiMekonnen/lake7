using System;
using System.Collections.Generic;

namespace lake7.Domain.Entities
{
    public class User : CommonEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        // Navigation collections
        public ICollection<Ride> Rides { get; set; } = new List<Ride>();
        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
