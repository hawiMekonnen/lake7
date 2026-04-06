using lake7.Domain.Entities;
using lake7.Application.Interface;

namespace lake7.Application.Services
{
    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;

        public RideService(IRideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }

        public async Task<Ride> RequestRideAsync(Ride ride)
        {
            ride.Status = "Requested";
            ride.RequestedAt = DateTime.UtcNow;
            return await _rideRepository.AddAsync(ride);
        }

        public async Task<List<Ride>> GetAllRidesAsync()
        {
            return (await _rideRepository.GetAllAsync()).ToList();
        }

        public async Task<Ride?> GetRideByIdAsync(Guid id)
        {
            return await _rideRepository.GetByIdAsync(id);
        }

        public async Task<Ride?> UpdateRideStatusAsync(Guid id, string status)
        {
            var ride = await _rideRepository.GetByIdAsync(id);
            if (ride == null) return null;

            ride.Status = status;
            ride.UpdatedAt = DateTime.UtcNow;
            return await _rideRepository.UpdateAsync(ride);
        }
    }
}
