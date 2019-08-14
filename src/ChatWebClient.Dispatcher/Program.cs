using ChatServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Linq;

namespace ChatWebClient.Dispatcher
{
    class Program
    {
        static readonly HttpClient _client = new HttpClient();
        static readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        static void Main(string[] args)
        {
            string webClientUri = ConfigurationManager.AppSettings["ChatWebClient.BaseUri"].TrimEnd('/');

            Console.WriteLine("[*] Waiting for {0}...", webClientUri);

            if(IsHostReachable(webClientUri).Result)
            {
                Console.WriteLine("[*] The host is up and running.");

                var chatServer = new Server();

                while(true)
                {
                    IEnumerable<string> messages = 
                        chatServer.ReadAllMessagesForClient(MvcApplication.ClientId);

                    PostMessagesToClient($"{webClientUri}/Chat/Messages", messages).GetAwaiter().GetResult();

                    Console.WriteLine("[*] Posted {0} messages to the web client.", messages.Count());

                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine("[ERROR] Host {0} could not be reached!");
                return;
            }
        }

        static async Task<bool> IsHostReachable(string uri)
        {
            HttpResponseMessage response = await _client.GetAsync(uri);

            Console.WriteLine("[*] Response status code: {0}", response.StatusCode);

            return response.IsSuccessStatusCode;
        }

        static async Task PostMessagesToClient(string uri, IEnumerable<string> messages)
        {
            var body = new StringContent(
                _serializer.Serialize(messages), 
                Encoding.UTF8, 
                "application/json"
            );

            HttpResponseMessage response = await _client.PostAsync(uri, body);

            response.EnsureSuccessStatusCode();
        }
    }
}
