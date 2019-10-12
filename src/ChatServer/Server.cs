using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatServer
{
    public class Server : IDisposable
    {
        private readonly MemoryMappedTextFile _clientsList;
        private readonly MemoryMappedTextFile _chatBuffer;

        public Server()
        {
            _clientsList = new MemoryMappedTextFile("ClientsListFile", 2048);
            _chatBuffer = new MemoryMappedTextFile("ChatBufferFile", 4096);
        }

        public bool Register(string clientId, out string message)
        {
            message = $"Client {clientId} has been registered successfully!";

            foreach(string registeredClientId in GetRegisteredClients())
            {
                if(registeredClientId.Equals(clientId, StringComparison.InvariantCultureIgnoreCase))
                {
                    message = $"Client {clientId} is already registered!";
                    return false;
                }
            }

            _clientsList.AppendText(clientId + Environment.NewLine);

            return true;
        }

        private IEnumerable<string> GetRegisteredClients()
        {
            return _clientsList.ReadAllText().Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries
            );
        }

        public void Send(Message message)
        {
            _chatBuffer.AppendText(message.ToString() + Environment.NewLine);
        }

        public IEnumerable<Message> ReadAllMessagesForClient(string clientId)
        {
            if(!GetRegisteredClients().Contains(clientId, StringComparer.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"Client {clientId} is not registered!");
            }

            IEnumerable<Message> allChatMessages = _chatBuffer.ReadAllText().Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries
            ).Select(msg => Message.ParseJson(msg));

            return allChatMessages.Where(msg => msg.IsAddressedTo(clientId));
        }

        public void Dispose()
        {
            _chatBuffer.Dispose();
            _clientsList.Dispose();
        }
    }
}
