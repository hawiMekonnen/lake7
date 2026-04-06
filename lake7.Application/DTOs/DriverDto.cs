using System;

namespace lake7.Application.DTOs
{
    public class DriverDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string VehicleInfo { get; set; }
        public required string LicensePlate { get; set; }
        public required string VehicleType { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public bool IsAvailable { get; set; }
        public double Rating { get; set; }
        public int CompletedRides { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
