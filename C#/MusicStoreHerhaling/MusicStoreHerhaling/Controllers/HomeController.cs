using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStoreHerhaling.Controllers
{
    public class HomeController : Controller
    {
        private HttpServerUtilityBase _utilityBase;
        public const string ROCK_URL = "https://www.youtube.com/playlist?list=PLhd1HyMTk3f5yqcPXjLo8qroWJiMMFBSk";

        public HomeController(HttpServerUtilityBase utilityBase)
        {
            _utilityBase = utilityBase;
        }

        public ActionResult Index()
        {
            return Content(ControllerNameAndAction());
        }

        public ActionResult About()
        {
            return Content(ControllerNameAndAction()); ;
        }

        public ActionResult Details(int id)
        {
            return Content(ControllerNameAndAction(id));
        }

        public ActionResult Search(string genre)
        {
            if (genre.ToLower().Equals("rock"))
                return RedirectPermanent(ROCK_URL);
            if (genre.ToLower().Equals("jazz"))
                return RedirectToAction("Index");
            return null;
        }

        private string ControllerNameAndAction(object param = null)
        {
            if (param == null)
                return $"{RouteData.Values["controller"]}:{RouteData.Values["action"]}";
            return $"{RouteData.Values["controller"]}:{RouteData.Values["action"]}:{param}";
        }
    }
}