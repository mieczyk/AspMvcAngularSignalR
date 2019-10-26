using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Messages(IEnumerable<MessageViewModel> msgViewModels)
        {
            if(msgViewModels.Any())
            {
                _hub.Notify(msgViewModels.Select(msg => msg.ToMessage()));
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}