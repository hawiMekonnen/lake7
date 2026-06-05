using lake7.Application.DTOs;
using lake7.Domain.Entities;

namespace lake7.Application.Helpers
{
    public static class RideMapper
    {
        public static RideDto ToDto(Ride ride)
        {
            var dropoffParts = (ride.DropoffLocation ?? string.Empty).Split('|');
            var cleanDropoff = dropoffParts[0];
            var vehicleType = dropoffParts.Length > 1 ? dropoffParts[1] : "Economy";

            var driverVehicleParts = (ride.Driver?.VehicleInfo ?? string.Empty).Split('|');
            var cleanVehicleInfo = driverVehicleParts.Length > 0 ? driverVehicleParts[0].Trim() : string.Empty;
            var driverProfilePic = driverVehicleParts.Length > 1 ? driverVehicleParts[1].Trim() : null;

            return new RideDto
            {
                Id = ride.Id,
                PickupLocation = ride.PickupLocation,
                DropoffLocation = cleanDropoff,
                Status = ride.Status,
                RequestedAt = ride.RequestedAt,
                UpdatedAt = ride.UpdatedAt,
                UserName = ride.User?.Name,
                UserPhoneNumber = ride.User?.PhoneNumber,
                PickupLatitude = ride.PickupLatitude,
                PickupLongitude = ride.PickupLongitude,
                DropoffLatitude = ride.DropoffLatitude,
                DropoffLongitude = ride.DropoffLongitude,
                VehicleType = vehicleType,
                DriverId = ride.DriverId,
                DriverName = ride.Driver?.Name,
                DriverPhoneNumber = ride.Driver?.PhoneNumber,
                DriverProfilePicture = driverProfilePic,
                DriverVehicleInfo = cleanVehicleInfo,
                DriverLicensePlate = ride.Driver?.LicensePlate
            };
        }
    }
}