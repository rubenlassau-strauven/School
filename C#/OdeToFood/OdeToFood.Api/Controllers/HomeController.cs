using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using OdeToFood.Business;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Controllers
{
    public class HomeController : Controller
    {
        public HttpCookieCollection _httpCookieCollection;
        public IApiProxy _apiProxy;

        public HomeController(IApiProxy _apiProxy)
        {
            this._httpCookieCollection = new HttpCookieCollection();
            this._apiProxy = _apiProxy;
        }

        public ActionResult About()
        {
            if (_httpCookieCollection["LastVisited"] == null)
            {
                ViewBag.LastVisit = "Never";
                HttpCookie lastVisited = new HttpCookie("LastVisited");
                lastVisited.Value = DateTime.Now.ToString();
                _httpCookieCollection.Add(lastVisited);
            }
            else
            {
                ViewBag.LastVisit = _httpCookieCollection["LastVisited"].Value;
                _httpCookieCollection["LastVisited"].Value = DateTime.Now.ToString();
            }
            return View("About");
        }

        public async Task<ActionResult> Index()
        {
            var model = await _apiProxy.GetReviewsAsync();
            return View(model);
        }

        public async Task<ActionResult> RestaurantDetails(int id)
        {
            var model = await _apiProxy.GetRestaurantByIdAsync(id);
            if (model == null)
                return new HttpNotFoundResult();
            return View(model);
        }

        public ActionResult Create()
        {
            return null;
        }

        public async Task<ActionResult> Create(Review review)
        {
            return null;
        }

        public async Task<ActionResult> Edit(int id)
        {
            return null;
        }

        public async Task<ActionResult> Edit(int id, Review review)
        {
            return null;
        }

        public async Task<ActionResult> Delete(int id)
        {
            return null;
        }

    }
}
