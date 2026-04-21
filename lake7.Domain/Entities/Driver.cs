using System;

namespace lake7.Domain.Entities
{
    public class Driver : CommonEntity
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string VehicleInfo { get; set; }
        public required string LicensePlate { get; set; }
        public required string VehicleType { get; set; }

        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsAvailable { get; set; } = true;
        public double Rating { get; set; } = 0;
        public int CompletedRides { get; set; } = 0;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastActiveAt { get; set; }
        
    }
}
