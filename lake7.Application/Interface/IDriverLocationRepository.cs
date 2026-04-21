using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IDriverLocationRepository
    {
        Task<DriverLocation?> GetByDriverIdAsync(Guid driverId);
        Task<DriverLocation> AddOrUpdateAsync(DriverLocation location);
        Task<List<DriverLocation>> GetNearbyDriversAsync(double latitude, double longitude, double radiusKm);
    }
}
