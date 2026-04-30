using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace lake7.WebAPI.Hubs
{
    [Authorize] // ensure only authenticated drivers connect
    public class DriverHub : Hub
    {
        // Called when driver connects and registers with their ID
        public async Task RegisterDriver(string driverId)
        {
            if (string.IsNullOrWhiteSpace(driverId))
            {
                throw new HubException("DriverId is required");
            }

            // Add driver to a group named after their ID
            await Groups.AddToGroupAsync(Context.ConnectionId, driverId);

            // Confirm back to caller
            await Clients.Caller.SendAsync("Registered", driverId);
        }

        public async Task SendRideRequest(string driverId, object rideData)
        {
            await Clients.Group(driverId).SendAsync("RideRequested", rideData);
        }

        //handle disconnect
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
