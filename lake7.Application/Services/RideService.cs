using lake7.Application.Helpers;
using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace lake7.Application.Services
{
    public class RideService : IRideService
    {
        // ... (rest of the class)
        private readonly IRideRepository _rideRepository;
        private readonly IDriverLocationService _driverLocationService;
        private readonly ILogger<RideService> _logger;
        private readonly INotificationService _notificationService;
        private readonly IPaymentService _paymentService;

        public RideService(
            IRideRepository rideRepository,
            IDriverLocationService driverLocationService,
            ILogger<RideService> logger,
            INotificationService notificationService,
            IPaymentService paymentService)
        {
            _rideRepository = rideRepository;
            _driverLocationService = driverLocationService;
            _logger = logger;
            _notificationService = notificationService;
            _paymentService = paymentService;
        }

        public async Task<Ride> RequestRideAsync(Ride ride)
        {
            ride.Status = RideStatus.Pending;
            ride.RequestedAt = DateTime.UtcNow;
            var savedRide = await _rideRepository.AddAsync(ride);

            // Notify all drivers in real time
            await _notificationService.NotifyAllDriversAsync(savedRide);

            return savedRide;
        }

        public async Task<List<Ride>> GetAllRidesAsync()
        {
            return (await _rideRepository.GetAllAsync()).ToList();
        }

        public async Task<Ride?> GetRideByIdAsync(Guid id)
        {
            return await _rideRepository.GetByIdAsync(id);
        }

        public async Task<Ride?> UpdateRideStatusAsync(Guid id, RideStatus status)
        {
            var ride = await _rideRepository.GetByIdAsync(id);
            if (ride == null) return null;

            ride.Status = status;
            ride.UpdatedAt = DateTime.UtcNow;
            return await _rideRepository.UpdateAsync(ride);
        }

        public async Task<Ride?> AcceptRideAsync(Guid rideId, Guid driverId)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);

            if (ride == null) return null;

            // If already accepted, lock it
            if (ride.Status == RideStatus.Accepted || ride.DriverId != null)
            {
                return null; // another driver already accepted
            }

            ride.DriverId = driverId;
            ride.Status = RideStatus.Accepted;
            ride.UpdatedAt = DateTime.UtcNow;

            var updatedRide = await _rideRepository.UpdateAsync(ride);
            
            // Notify the user that their ride has been accepted
            if (updatedRide != null)
            {
                var rideDto = RideMapper.ToDto(updatedRide);
                await _notificationService.NotifyUserAsync(updatedRide.UserId, rideDto);
            }

            return updatedRide;
        }

        public async Task<(Ride ride, List<DriverLocation> nearbyDrivers)> RequestRideWithMatchingAsync(Ride ride, double radiusKm)
        {
            ride.Status = RideStatus.Pending;
            ride.RequestedAt = DateTime.UtcNow;
            var savedRide = await _rideRepository.AddAsync(ride);

            var nearbyDrivers = await _driverLocationService.GetNearbyDriversAsync(
                ride.PickupLatitude, ride.PickupLongitude, radiusKm);

            foreach (var driver in nearbyDrivers)
            {
                await _notificationService.NotifyDriverAsync(driver.DriverId, savedRide);
                _logger.LogInformation($"Notified driver {driver.DriverId} for ride {ride.Id}");
            }

            return (savedRide, nearbyDrivers);
        }

        public async Task<Ride?> TransitionRideStatusAsync(Guid rideId, RideStatus newStatus)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null) return null;

            bool valid = (ride.Status, newStatus) switch
            {
                (RideStatus.Pending, RideStatus.Accepted) => true,
                (RideStatus.Accepted, RideStatus.InProgress) => true,
                (RideStatus.InProgress, RideStatus.Completed) => true,
                (RideStatus.Pending, RideStatus.Cancelled) => true,
                (RideStatus.Accepted, RideStatus.Cancelled) => true,
                (RideStatus.InProgress, RideStatus.Cancelled) => true,
                _ => false
            };

            if (!valid) return null; // invalid transition

            ride.Status = newStatus;
            ride.UpdatedAt = DateTime.UtcNow;

            if (newStatus == RideStatus.Completed)
            {
                ride.CompletedAt = DateTime.UtcNow;
                // Handle transaction
                // For simplicity, we use a fixed amount or calculate based on distance.
                // Assuming amount is handled elsewhere or we use a default for now.
                decimal amount = 50.0m; // Example amount
                await _paymentService.ProcessPaymentAsync(ride.UserId, Guid.Empty, ride.Id, amount, "Wallet");
            }

            return await _rideRepository.UpdateAsync(ride);
        }

        public async Task<List<Ride>> GetNearbyPendingRidesAsync(double latitude, double longitude, double radiusKm)
        {
            var pendingRides = await _rideRepository.GetPendingRidesAsync();

            return pendingRides
                .Where(r => LocationHelper.CalculateDistance(latitude, longitude, r.PickupLatitude, r.PickupLongitude) <= radiusKm)
                .ToList();
        }
    }
}
