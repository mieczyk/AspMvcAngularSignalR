using System.Collections.Generic;
using ChatServer;
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

        public void Notify(IEnumerable<Message> messages)
        {
            // Check what other options we have
            _context.Clients.All.onMessagesReceived(messages);
        }

        public void Send(MessageViewModel msgViewModel)
        {
            msgViewModel.Sender = MvcApplication.ClientId;

            MvcApplication.ChatServer.Send(msgViewModel.ToMessage());
        }
    }
}