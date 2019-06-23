using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ChatServer
{
    public class Server : IDisposable
    {
        private const string _separator = ":@:";

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

        public void Send(string message, string senderId, string recipientId = null)
        {
            StringBuilder chatLine = new StringBuilder(senderId + _separator);

            if(!string.IsNullOrWhiteSpace(recipientId))
            {
                chatLine.Append(recipientId + _separator);
            }

            chatLine.Append(message);

            _chatBuffer.AppendText(chatLine + Environment.NewLine);
        }

        public IEnumerable<string> ReadAllMessagesForClient(string clientId)
        {
            if(!GetRegisteredClients().Contains(clientId, StringComparer.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"Client {clientId} is not registered!");
            }

            IEnumerable<Message> allChatMessages = _chatBuffer.ReadAllText().Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries
            ).Select(msg => Message.Parse(msg));

            var clientMessages = new List<string>(allChatMessages.Count());

            foreach(Message msg in allChatMessages)
            {
                if(msg.IsAddressedTo(clientId))
                {
                    clientMessages.Add($"{msg.Sender}: {msg.Body}");
                }
            }

            return clientMessages;
        }

        public void Dispose()
        {
            _chatBuffer.Dispose();
            _clientsList.Dispose();
        }

        private class Message
        {
            public string Sender { get; }
            public string Recipient { get; }
            public string Body { get; }

            private Message(string sender, string recipient, string body)
            {
                Sender = sender;
                Recipient = recipient;
                Body = body;
            }

            public bool IsAddressedTo(string clientId)
            {
                return string.IsNullOrWhiteSpace(Recipient) 
                    || Recipient.Equals(clientId, StringComparison.InvariantCultureIgnoreCase);
            }

            public static Message Parse(string message)
            {
                string[] msgParts = message.Split(
                    new[] { _separator }, 
                    StringSplitOptions.None
                );

                bool isMessageDirect = msgParts.Length > 2;

                return new Message(
                    msgParts[0],
                    isMessageDirect ? msgParts[1] : null,
                    isMessageDirect ? msgParts[2] : msgParts[1]
                );
            }
        }
    }
}
