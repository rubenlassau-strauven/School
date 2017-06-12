using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OdeToFood.Business;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Controllers
{
    public class HomeController : Controller
    {
        private IApiProxy _apiProxy;
        public const string LAST_VISIT_COOKIE_NAME = "LastVisit";
        public const string NEVER_VISITED_TEXT = "Never";

        public HomeController(IApiProxy apiProxy)
        {
            _apiProxy = apiProxy;
            _apiProxy.RegisterAsDummyUserAndUseBearerToken();
        }

        public ActionResult About()
        {
            ViewBag.LastVisit = Request.Cookies[LAST_VISIT_COOKIE_NAME] == null
                ? NEVER_VISITED_TEXT
                : Request.Cookies[LAST_VISIT_COOKIE_NAME].Value;
            HttpCookie lastVisitCookie = new HttpCookie(LAST_VISIT_COOKIE_NAME);
            lastVisitCookie.Value = DateTime.Now.ToString();
            Response.Cookies.Set(lastVisitCookie);
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
       {        
            var reviews = await _apiProxy.GetReviewsAsync();
            return View(reviews);
        }

        [HttpGet]
        public async Task<ActionResult> RestaurantDetails(int id)
        {
            var restaurant = await _apiProxy.GetRestaurantByIdAsync(id);
            return View(restaurant);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "New Review";
            return View("Edit");
        }

        [HttpPost]
        public async Task<ActionResult> Create(Review review)
        {
            await _apiProxy.PostReviewAsync(review);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var review = await _apiProxy.GetReviewByIdAsync(id);
            ViewBag.Title = "Edit Review";
            return View(review);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, Review review)
        {
            var updateSucceeded = _apiProxy.PutReviewAsync(id, review);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var deleteSucceeded = await _apiProxy.DeleteReviewAsync(id);
            return RedirectToAction("Index");
        }
    }
}
