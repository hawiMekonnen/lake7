using lake7.Application.DTOs;
using lake7.Domain.Entities;

namespace lake7.Application.Helpers
{
    public static class RideMapper
    {
        public static RideDto ToDto(Ride ride)
        {
            return new RideDto
            {
                Id = ride.Id,
                PickupLocation = ride.PickupLocation,
                DropoffLocation = ride.DropoffLocation,
                Status = ride.Status
            };
        }
    }
}