using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace lake7.WebAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<DriverHub> _hubContext;

        public NotificationService(IHubContext<DriverHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyDriverAsync(Guid driverId, Ride savedRide)
        {
            await _hubContext.Clients.Group(driverId.ToString()).SendAsync("RideRequested", savedRide);
        }

        public async Task NotifyAllDriversAsync(object rideData)
        {
            await _hubContext.Clients.All.SendAsync("RideRequested", rideData);
        }
    }
}
