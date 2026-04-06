using System;

namespace lake7.Domain.Entities
{
    public class Driver
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string VehicleInfo { get; set; }
        public required string LicensePlate { get; set; }
        public required string VehicleType { get; set; }

        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }

        public bool IsAvailable { get; set; } = true;
        public double Rating { get; set; } = 0;
        public int CompletedRides { get; set; } = 0;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastActiveAt { get; set; }
    }
}
