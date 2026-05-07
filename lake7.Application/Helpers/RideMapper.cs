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
                Status = ride.Status,
                UserName = ride.User?.Name,
                UserPhoneNumber = ride.User?.PhoneNumber,
                PickupLatitude = ride.PickupLatitude,
                PickupLongitude = ride.PickupLongitude,
                DropoffLatitude = ride.DropoffLatitude,
                DropoffLongitude = ride.DropoffLongitude
            };
        }
    }
}