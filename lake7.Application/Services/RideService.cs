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
        private readonly IDriverRepository _driverRepository;
        private readonly IDriverLocationService _driverLocationService;
        private readonly ILogger<RideService> _logger;
        private readonly INotificationService _notificationService;
        private readonly IPaymentService _paymentService;

        public RideService(
            IRideRepository rideRepository,
            IDriverRepository driverRepository,
            IDriverLocationService driverLocationService,
            ILogger<RideService> logger,
            INotificationService notificationService,
            IPaymentService paymentService)
        {
            _rideRepository = rideRepository;
            _driverRepository = driverRepository;
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
            await _notificationService.NotifyAllDriversAsync(RideMapper.ToDto(savedRide));

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
            
            // Explicitly load the driver so the navigation property isn't null for the SignalR message
            if (updatedRide != null)
            {
                // We fetch the full updated ride again to ensure navigation properties are populated
                var fullyLoadedRide = await _rideRepository.GetByIdAsync(updatedRide.Id);
                
                if (fullyLoadedRide != null)
                {
                    // Manually populate the Driver property if EF Core caching missed it
                    if (fullyLoadedRide.Driver == null)
                    {
                        fullyLoadedRide.Driver = await _driverRepository.GetByIdAsync(driverId);
                    }

                    var rideDto = RideMapper.ToDto(fullyLoadedRide);
                    await _notificationService.NotifyUserAsync(fullyLoadedRide.UserId, rideDto);
                    return fullyLoadedRide;
                }
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

            var rideDto = RideMapper.ToDto(savedRide);
            foreach (var driver in nearbyDrivers)
            {
                await _notificationService.NotifyDriverAsync(driver.DriverId, rideDto);
                _logger.LogInformation($"Notified driver {driver.DriverId} for ride {ride.Id}");
            }

            return (savedRide, nearbyDrivers);
        }

        public async Task<Ride?> TransitionRideStatusAsync(Guid rideId, RideStatus newStatus, decimal? finalFare = null)
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
                decimal amount = finalFare ?? 50.0m;
                ride.Fare = (double)amount;
                try 
                {
                    await _paymentService.ProcessPaymentAsync(ride.UserId, null, ride.Id, amount, "Wallet");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Could not save payment record");
                }
                
                // Notify User of final price
                await _notificationService.NotifyUserRideCompletedAsync(ride.UserId, new
                {
                    Type = "RideCompleted",
                    Message = $"Your ride has been completed. Final fare: ETB {amount:F2}",
                    RideId = ride.Id,
                    FinalFare = amount
                });
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
