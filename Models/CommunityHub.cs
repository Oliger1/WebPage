using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebSiteDocuments.Models
{
    public class CommunityHub : Hub
    {
        public async Task SendMessage(string userName, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }
    }
}
