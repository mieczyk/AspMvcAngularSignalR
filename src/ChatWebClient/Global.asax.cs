using ChatServer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChatWebClient
{
    public class MvcApplication : HttpApplication
    {
        public const string ClientId = "ChatWebClient";
        public static Server ChatServer { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ChatServer = new Server();

            if(!ChatServer.Register(ClientId, out string message))
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
