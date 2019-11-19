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
            _context.Clients.All.onMessagesReceived(messages);
        }

        public void Send(string messageString)
        {
            var message = new Message(
                MvcApplication.ClientId,
                messageString,
                messageString.TryExtractRecipient()
            );

            MvcApplication.ChatServer.Send(message);
        }
    }
}