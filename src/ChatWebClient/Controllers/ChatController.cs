using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace ChatWebClient.Controllers
{
    public class ChatController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Messages(IEnumerable<string> messages)
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}