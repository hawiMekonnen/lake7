using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace lake7.WebAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<DriverHub> _hubContext;
        private readonly IHubContext<UserHub> _userHubContext;

        public NotificationService(IHubContext<DriverHub> hubContext, IHubContext<UserHub> userHubContext)
        {
            _hubContext = hubContext;
            _userHubContext = userHubContext;
        }

        public async Task NotifyDriverAsync(Guid driverId, object savedRide)
        {
            await _hubContext.Clients.Group(driverId.ToString()).SendAsync("RideRequested", savedRide);
        }

        public async Task NotifyAllDriversAsync(object rideData)
        {
            await _hubContext.Clients.All.SendAsync("RideRequested", rideData);
        }

        public async Task NotifyUserAsync(Guid userId, object data)
        {
            await _userHubContext.Clients.Group(userId.ToString()).SendAsync("RideAccepted", data);
        }

        public async Task NotifyUserRideCompletedAsync(Guid userId, object data)
        {
            await _userHubContext.Clients.Group(userId.ToString()).SendAsync("RideCompleted", data);
        }

        public async Task NotifyOrderCreatedAsync(object orderData)
        {
            await _userHubContext.Clients.All.SendAsync("OrderCreated", orderData);
        }

        public async Task NotifyOrderAssignedAsync(Guid driverId, object orderData)
        {
            await _hubContext.Clients.Group(driverId.ToString()).SendAsync("OrderAssigned", orderData);
        }

        public async Task NotifyOrderStatusChangedAsync(Guid userId, string status)
        {
            await _userHubContext.Clients.Group(userId.ToString()).SendAsync("OrderStatusChanged", status);
        }

        public async Task NotifyOrderPreparedAsync(object orderData)
        {
            await _hubContext.Clients.All.SendAsync("OrderPrepared", orderData);
        }
    }
}
