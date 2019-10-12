using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(ChatWebClient.Startup))]

namespace ChatWebClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}