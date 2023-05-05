using IdentityServer4.Models;
using Microsoft.AspNetCore.SignalR;

namespace UtilityComplaints.WebUI.Models
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
