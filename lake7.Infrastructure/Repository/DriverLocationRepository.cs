using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace lake7.Infrastructure.Repositories
{
    public class DriverLocationRepository : IDriverLocationRepository
    {
        private readonly Lake7DbContext _context;

        public DriverLocationRepository(Lake7DbContext context)
        {
            _context = context;
        }

        public async Task<DriverLocation?> GetByDriverIdAsync(Guid driverId)
        {
            return await _context.DriverLocations
                .FirstOrDefaultAsync(dl => dl.DriverId == driverId);
        }

        public async Task<DriverLocation> AddOrUpdateAsync(DriverLocation location)
        {
            var existing = await GetByDriverIdAsync(location.DriverId);
            if (existing == null)
            {
                _context.DriverLocations.Add(location);
            }
            else
            {
                existing.Latitude = location.Latitude;
                existing.Longitude = location.Longitude;
                existing.LastUpdated = DateTime.UtcNow;
                _context.DriverLocations.Update(existing);
            }

            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<List<DriverLocation>> GetNearbyDriversAsync(double latitude, double longitude, double radiusKm)
        {
            var allDrivers = await _context.DriverLocations.ToListAsync();

            return allDrivers
                .Where(d => CalculateDistance(latitude, longitude, d.Latitude, d.Longitude) <= radiusKm)
                .ToList();
        }

        // Haversine formula for distance in km
        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in km
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
