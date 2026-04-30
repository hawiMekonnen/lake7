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
    }
}
