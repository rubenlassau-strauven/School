using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private HttpServerUtilityBase serverUtility;

        public HomeController(HttpServerUtilityBase serverUtility)
        {
            this.serverUtility = serverUtility;
        }

        public const string ROCK_URL = @"https://www.youtube.com/playlist?list=PLhd1HyMTk3f5yqcPXjLo8qroWJiMMFBSk";

        public ActionResult Index()
        {
            return Content(ControllerNameActionNameAndParam());
        }

        public ActionResult About()
        {
            return Content(ControllerNameActionNameAndParam());
        }

        public ActionResult Details(int id)
        {
            return Content(ControllerNameActionNameAndParam(id));
        }

        public ActionResult Search(string genre)
        {
            if (genre.Equals("Rock"))
                return RedirectPermanent(ROCK_URL);
            if (genre.Equals("Jazz"))
                return RedirectToAction("Index");
            if (genre.Equals("Metal"))
                return RedirectToAction("Details",new { id = new Random().Next()});
            if (genre.Equals("Classic"))
            {
                string path = serverUtility.MapPath("~/Content/Site.css");
                byte[] file = System.IO.File.ReadAllBytes(path);
                return File(file,"css");
            }
            return Content(ControllerNameActionNameAndParam(genre));
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private string ControllerNameActionNameAndParam(object param = null)
        {
            if (param == null)
                return RouteData.Values["controller"] + ":" + RouteData.Values["action"];
            return RouteData.Values["controller"] + ":" + RouteData.Values["action"] + ":" + param;
        }
    }
}