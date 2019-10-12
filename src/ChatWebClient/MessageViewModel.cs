using ChatServer;

namespace ChatWebClient
{
    public class MessageViewModel
    {
        public string Sender { get; set; }
        public string Body { get; set; }
        public string Recipient { get; set; }

        public Message ToMessage()
        {
            return new Message(Sender, Body, Recipient);
        }
    }
}