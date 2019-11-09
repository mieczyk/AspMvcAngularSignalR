using ChatServer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ChatConsoleClient
{
    class Program
    {
        readonly string _clientId;
        SynchronizedCollection<char> _msgBody;
        IEnumerable<Message> _allMessages;

        internal Program(string clientId)
        {
            _clientId = clientId;
            _msgBody = new SynchronizedCollection<char>();
            _allMessages = new List<Message>();
        }

        void Run()
        {
            using (var server = new Server())
            {
                if (!server.Register(_clientId, out string message))
                {
                    Console.WriteLine($"Could not register {_clientId} at the chat server.");
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
                        IEnumerable<Message> newMessages = server.ReadAllMessagesForClient(_clientId);

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
                            _clientId, 
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

            Console.WriteLine($"Welcome {_clientId}!");
            Console.WriteLine(new string('=', Console.BufferWidth));

            foreach (Message msg in _allMessages)
            {
                Console.WriteLine($"{msg.Sender}: {msg.Body}");
            }
            Console.WriteLine(new string('=', Console.BufferWidth));

            Console.Write($"{_clientId}: ");
            foreach(char character in _msgBody)
            {
                Console.Write(character);
            }
        }

        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("Client ID required as a parameter.");
                return;
            }

            new Program(args[0]).Run();
        }
    }
}
