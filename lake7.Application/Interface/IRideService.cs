using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IRideService
    {
        Task<Ride> RequestRideAsync(Ride ride);
        Task<List<Ride>> GetAllRidesAsync();
        Task<Ride?> GetRideByIdAsync(Guid id);
        Task<Ride?> UpdateRideStatusAsync(Guid id, string status);
    }
}
