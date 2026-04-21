using lake7.Application.Interface;
using lake7.Domain.Entities;

namespace lake7.Application.Services
{
    public class DriverLocationService : IDriverLocationService
    {
        private readonly IDriverLocationRepository _driverLocationRepository;

        public DriverLocationService(IDriverLocationRepository driverLocationRepository)
        {
            _driverLocationRepository = driverLocationRepository;
        }

        public async Task<DriverLocation?> GetLocationByDriverIdAsync(Guid driverId)
        {
            return await _driverLocationRepository.GetByDriverIdAsync(driverId);
        }

        public async Task<DriverLocation> UpdateLocationAsync(Guid driverId, double latitude, double longitude)
        {
            var location = new DriverLocation
            {
                DriverId = driverId,
                Latitude = latitude,
                Longitude = longitude,
                LastUpdated = DateTime.UtcNow
            };

            return await _driverLocationRepository.AddOrUpdateAsync(location);
        }

        public async Task<List<DriverLocation>> GetNearbyDriversAsync(double latitude, double longitude, double radiusKm)
        {
            return await _driverLocationRepository.GetNearbyDriversAsync(latitude, longitude, radiusKm);
        }
    }
}
