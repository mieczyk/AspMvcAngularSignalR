using Newtonsoft.Json;
using System;

namespace ChatServer
{
    public class Message
    {
        public string Sender { get; }
        public string Body { get; }
        public string Recipient { get; }

        public Message(string sender, string body, string recipient = null)
        {
            Sender = sender;
            Body = body;
            Recipient = recipient;
        }

        public bool IsAddressedTo(string clientId)
        {
            return string.IsNullOrWhiteSpace(Recipient)
                || Recipient.Equals(clientId, StringComparison.InvariantCultureIgnoreCase);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Message ParseJson(string messageJson)
        {
            return JsonConvert.DeserializeObject<Message>(messageJson);
        }
    }
}
