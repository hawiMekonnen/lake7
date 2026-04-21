using lake7.Domain.Entities;
using lake7.Domain.Enums;

namespace lake7.Application.Interface
{
    public interface IRideService
    {
        Task<Ride> RequestRideAsync(Ride ride);
        Task<List<Ride>> GetAllRidesAsync();
        Task<Ride?> GetRideByIdAsync(Guid id);
        Task<Ride?> UpdateRideStatusAsync(Guid id, RideStatus status);
        Task<Ride?> AcceptRideAsync(Guid rideId, Guid driverId);
        Task<Ride?> TransitionRideStatusAsync(Guid rideId, RideStatus newStatus);

        Task<(Ride ride, List<DriverLocation> nearbyDrivers)> RequestRideWithMatchingAsync(Ride ride, double radiusKm);
    }
}
