using lake7.Domain.Entities;
using lake7.Application.DTOs;

namespace lake7.Application.Helpers
{
    public static class DriverMapper
    {
        public static DriverDto ToDto(Driver driver)
        {
            return new DriverDto
            {
                Id = driver.Id,
                Name = driver.Name,
                VehicleInfo = driver.VehicleInfo,
                LicensePlate = driver.LicensePlate,
                VehicleType = driver.VehicleType,
                Email = driver.Email,
                PhoneNumber = driver.PhoneNumber,
                IsAvailable = driver.IsAvailable,
                Rating = driver.Rating,
                CompletedRides = driver.CompletedRides,
                RegisteredAt = driver.RegisteredAt,
                UpdatedAt = driver.UpdatedAt
            };
        }
    }
}
