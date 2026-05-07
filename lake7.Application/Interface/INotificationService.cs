
using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface INotificationService
    {
        
        Task NotifyAllDriversAsync(object rideData);
        Task NotifyDriverAsync(Guid driverId, Ride savedRide);
        Task NotifyUserAsync(Guid userId, object data);
    }
}

