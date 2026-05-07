using Microsoft.AspNetCore.SignalR;

namespace lake7.WebAPI.Hubs
{
    public class UserHub : Hub
    {
        public async Task RegisterUser(Guid userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }
    }
}
