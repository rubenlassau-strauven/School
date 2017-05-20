using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public IApiProxy _apiProxy;

        public HomeController(IApiProxy _apiProxy)
        {
            this._apiProxy = _apiProxy;
        }

        public ActionResult About()
        {
            ViewBag.LastVisit = "Never";
            HttpCookie lastVisited = new HttpCookie("LastVisited");
            lastVisited.Value = DateTime.Now.ToString();
            Response.Cookies.Add(lastVisited);
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
            ViewBag.Title = "New Review";
            return View("Edit");
        }

        public async Task<ActionResult> Create(Review review)
        {
            var postSucceeded = await _apiProxy.PostReviewAsync(review);
            if (!postSucceeded)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request: Invalid review");
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var review = await _apiProxy.GetReviewByIdAsync(id);
            if (review == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request: Review not found");
            return View(review);
        }

        public async Task<ActionResult> Edit(int id, Review review)
        {
            var putSucceeded = await _apiProxy.PutReviewAsync(id, review);
            if (!putSucceeded)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request: Invalid review");
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            return null;
        }

    }
}
