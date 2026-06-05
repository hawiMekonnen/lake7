
using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface INotificationService
    {
        
        Task NotifyAllDriversAsync(object rideData);
        Task NotifyDriverAsync(Guid driverId, object savedRide);
        Task NotifyUserAsync(Guid userId, object data);
        Task NotifyUserRideCompletedAsync(Guid userId, object data);

        // Order Notifications
        Task NotifyOrderCreatedAsync(object orderData);
        Task NotifyOrderAssignedAsync(Guid driverId, object orderData);
        Task NotifyOrderStatusChangedAsync(Guid userId, string status);
        Task NotifyOrderPreparedAsync(object orderData);
    }
}


