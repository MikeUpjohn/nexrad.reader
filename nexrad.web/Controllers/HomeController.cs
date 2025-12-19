using System.Web.Mvc;

namespace nexrad.web.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}