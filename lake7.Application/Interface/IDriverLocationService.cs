using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IDriverLocationService
    {
        Task<DriverLocation?> GetLocationByDriverIdAsync(Guid driverId);
        Task<DriverLocation> UpdateLocationAsync(Guid driverId, double latitude, double longitude);
        Task<List<DriverLocation>> GetNearbyDriversAsync(double latitude, double longitude, double radiusKm);
    }
}
