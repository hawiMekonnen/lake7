using Microsoft.AspNetCore.SignalR;

namespace lake7.WebAPI.Hubs
{
    public class UserHub : Hub
    {
        public async Task RegisterUser(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task NotifyOrderCreated(object order)
        {
            await Clients.All.SendAsync("OrderCreated", order);
        }

        public async Task NotifyOrderStatusChanged(string orderId, string status)
        {
            await Clients.All.SendAsync("OrderStatusChanged", new { orderId, status });
        }
    }
}

