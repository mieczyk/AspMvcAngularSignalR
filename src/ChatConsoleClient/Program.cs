using ChatServer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ChatConsoleClient
{
    class Program
    {
        const string ClientId = "ConsoleClient";

        SynchronizedCollection<char> _msgBody;
        IEnumerable<Message> _allMessages;

        internal Program()
        {
            _msgBody = new SynchronizedCollection<char>();
            _allMessages = new List<Message>();
        }

        void Run()
        {
            using (var server = new Server())
            {
                if (!server.Register(ClientId, out string message))
                {
                    Console.WriteLine($"Could not register {ClientId} at the chat server.");
                    Console.WriteLine(message);

                    return;
                }

                var msgBodyString = string.Empty;
                bool listenForNewMessages = true;

                // Check if new messages came.
                Task listenerTask = Task.Factory.StartNew(() =>
                {
                    while (listenForNewMessages)
                    {
                        IEnumerable<Message> newMessages = server.ReadAllMessagesForClient(ClientId);

                        if (newMessages.Count() > _allMessages.Count())
                        {
                            _allMessages = newMessages;
                            RefreshWindow();
                        }

                        Task.Delay(1000).Wait();
                    }
                });

                while (!msgBodyString.Equals("/exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    RefreshWindow();

                    // Read message char by char until ENTER is pressed.
                    while (true)
                    {
                        char readChar = Console.ReadKey().KeyChar;

                        // RETURN (ENTER)
                        if (readChar == (char)13)
                        {
                            break;
                        }

                        // BACKSPACE
                        if (readChar == (char)8)
                        {
                            if (_msgBody.Any())
                            {
                                _msgBody.RemoveAt(_msgBody.Count - 1);
                            }

                            RefreshWindow();
                            
                            continue;
                        }

                        _msgBody.Add(readChar);
                    }

                    msgBodyString = new string(_msgBody.ToArray());
                    _msgBody.Clear();
                    Console.Clear();

                    if (!msgBodyString.IsEmpty())
                    {
                        server.Send(new Message(
                            ClientId, 
                            msgBodyString, 
                            msgBodyString.TryExtractRecipient()
                        ));
                    }
                }

                listenForNewMessages = false;
                listenerTask.Wait();
            }
        }

        void RefreshWindow()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            foreach(Message msg in _allMessages)
            {
                Console.WriteLine($"{msg.Sender}: {msg.Body}");
            }
            Console.WriteLine(new string('=', Console.BufferWidth));

            Console.Write($"{ClientId}: ");
            foreach(char character in _msgBody)
            {
                Console.Write(character);
            }
        }

        static void Main(string[] args)
        {
            new Program().Run();
        }
    }
}
