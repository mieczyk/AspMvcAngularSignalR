using Microsoft.AspNet.SignalR;

namespace ChatWebClient
{
    public class MessagesHub : Hub
    {
        private readonly IHubContext _context;

        public MessagesHub()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
        }

        public void Notify(string message)
        {
            _context.Clients.All.onMessageReceived(message);
        }
    }

}