using System.Net;
using System.Web.Mvc;

namespace ChatWebClient.Controllers
{
    public class ChatController : Controller
    {
        private readonly MessagesHub _hub;

        public ChatController()
        {
            _hub = new MessagesHub();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(string message)
        {
            if(!string.IsNullOrWhiteSpace(message))
            {
                _hub.Notify(message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}