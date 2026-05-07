using lake7.Domain.Enums;

namespace lake7.Application.DTOs
{
    public class RideDto
    {
        public Guid Id { get; set; }
        public required string PickupLocation { get; set; }
        public required string DropoffLocation { get; set; }
        public RideStatus Status { get; set; } = RideStatus.Pending;
        public DateTime RequestedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DriverId { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string? UserName { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }
    }
}
