using ChatServer;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChatWebClient
{
    public class MvcApplication : HttpApplication
    {
        public static string ClientId { get; private set; }
        public static Server ChatServer { get; private set; }

        protected void Application_Start()
        {
            ClientId = ConfigurationManager.AppSettings["ClientId"];

            if (ClientId.IsEmpty())
            {
                throw new ArgumentNullException("Missing 'ClientId' setting in Web.config.");
            }

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ChatServer = new Server();

            if (!ChatServer.Register(ClientId, out string message))
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
