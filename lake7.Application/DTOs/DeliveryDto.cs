using lake7.Domain.Enums;

namespace lake7.Application.DTOs
{
    public class DeliveryDto
    {
        public Guid Id { get; set; }
        public required string PickupLocation { get; set; }
        public required string DropoffLocation { get; set; }
        public required string PackageDescription { get; set; }
        public RideStatus Status { get; set; } = RideStatus.Pending;
    }
}
