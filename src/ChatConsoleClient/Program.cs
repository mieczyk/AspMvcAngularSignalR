using ChatServer;
using System;

namespace ChatConsoleClient
{
    class Program
    {
        private const string ClientId = "ConsoleClient";

        static void Main(string[] args)
        {
            using (var server = new Server())
            {
                if (!server.Register(ClientId, out string message))
                {
                    Console.WriteLine($"Could not register {ClientId} at the chat server.");
                    Console.WriteLine(message);

                    return;
                }

                string msgBody = string.Empty;

                while(!msgBody.Equals("/exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.Write($"{ClientId}: ");
                    msgBody = Console.ReadLine();

                    server.Send(new Message(ClientId, msgBody, msgBody.TryExtractRecipient()));
                }
            }
        }
    }
}
