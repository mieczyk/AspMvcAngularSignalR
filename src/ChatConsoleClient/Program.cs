using ChatServer;
using System;

namespace ChatConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            const string clientId = "ConsoleClient";

            var server = new Server();

            server.Register(clientId, out string message);

            Console.ReadKey();
            server.Send(new Message(clientId, "Hello World!"));
        }
    }
}
