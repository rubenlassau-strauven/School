using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using OdeToFood.Api;
using OdeToFood.Api.Controllers;
using OdeToFood.Api.Tests.TestServices;
using Assert = NUnit.Framework.Assert;
using OdeToFood.Business;
using OdeToFood.Data.DomainClasses;
using RedirectToRouteResult = System.Web.Mvc.RedirectToRouteResult;

namespace OdeToFood.Api.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        private TestableHomeController _controller;
        private ReviewBuilder _reviewBuilder = new ReviewBuilder();
        private RestaurantBuilder _restaurantBuilder = new RestaurantBuilder();

        [SetUp]
        public void SetUp()
        {
            _controller = TestableHomeController.CreateInstance();
        }

        [Test]
        public void About_Firstvisit_ReturnsViewWithoutLastVisitedDateAndSavesFirstVisitToACookie()
        {
            //Arrange

            //Act
            var result = _controller.About() as ViewResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewBag.LastVisit, Is.EqualTo(HomeController.NEVER_VISITED_TEXT));
            Assert.That(_controller.Response.Cookies[HomeController.LAST_VISIT_COOKIE_NAME], Is.Not.Null);
        }

        [Test]
        public void About_SecondVisit_ReturnsViewWithLastVisitedDateAndSavesNewVisitToACookie()
        {
            //Arrange
            var cookie = new HttpCookie(HomeController.LAST_VISIT_COOKIE_NAME);
            cookie.Value = DateTime.Now.AddDays(-1).ToString();
            _controller.Request.Cookies.Set(cookie);

            //Act
            var result = _controller.About() as ViewResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewBag.LastVisit, Is.EqualTo(cookie.Value));
            Assert.That(_controller.Response.Cookies[HomeController.LAST_VISIT_COOKIE_NAME], Is.Not.Null);
        }

        [Test]
        public void Index_ReturnsIndexViewWithListOfAllReviews()
        {
            //Arrange
            IEnumerable<Review> reviews = _reviewBuilder.BuildList(5);
            _controller._apiProxyMock.Setup(proxy => proxy.GetReviewsAsync()).Returns(Task.FromResult(reviews));

            //Act
            var result = _controller.Index().Result as ViewResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EquivalentTo(reviews));
            _controller._apiProxyMock.Verify(proxy => proxy.GetReviewsAsync(), Times.Once);
        }

        [Test]
        public void RestaurantDetails_ExistingId_ReturnsViewWithRestaurantDetails()
        {
            //Arrange
            var restaurant = _restaurantBuilder.WithId().Build();
            _controller._apiProxyMock.Setup(proxy => proxy.GetRestaurantByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(restaurant));

            //Act
            var result = _controller.RestaurantDetails(restaurant.Id).Result as ViewResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(restaurant));
            _controller._apiProxyMock.Verify(proxy => proxy.GetRestaurantByIdAsync(restaurant.Id), Times.Once);
        }

        [Test]
        public void RestaurantDetails_NonExistingId_ReturnsNotFoundResult()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.GetRestaurantByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Restaurant>(null));

            //Act
            var result = _controller.RestaurantDetails(RandomGenerator.Integer()).Result as HttpNotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._apiProxyMock.Verify(proxy => proxy.GetRestaurantByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Create_ReturnsEditView()
        {
            //Arrange

            //Act
            var result = _controller.Create() as ViewResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewBag.Title, Is.EqualTo("New Review"));
            Assert.That(result.ViewName, Is.EqualTo("Edit"));
        }

        [Test]
        public void Create_ValidReviewModel_ReturnsRedirectToRouteResult()
        {
            //Arrange
            var review = _reviewBuilder.Build();
            _controller._apiProxyMock.Setup(proxy => proxy.PostReviewAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult(true));

            //Act
            var result = _controller.Create(review).Result as RedirectToRouteResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
            _controller._apiProxyMock.Verify(proxy => proxy.PostReviewAsync(review), Times.Once);
        }

        [Test]
        public void Create_InvalidReviewModel_ReturnsBadRequestResult()
        {
            //Arrange
            var review = _reviewBuilder.Build();
            _controller.ModelState.AddModelError("ReviewerName", "A review must have a reviewer");

            //Act
            var result = _controller.Create(review).Result as HttpStatusCodeResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            _controller._apiProxyMock.Verify(proxy => proxy.PostReviewAsync(review), Times.Once);
        }

        [Test]
        public void Edit_ExistingId_ReturnsEditViewWithReviewDetails()
        {
            //Arrange
            var review = _reviewBuilder.WithId().Build();
            _controller._apiProxyMock.Setup(proxy => proxy.GetReviewByIdAsync(review.Id))
                .Returns(Task.FromResult(review));

            //Act
            var result = _controller.Edit(review.Id).Result as ViewResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(review));
            Assert.That(result.ViewBag.Title, Is.EqualTo("Edit Review"));
            _controller._apiProxyMock.Verify(proxy => proxy.GetReviewByIdAsync(review.Id), Times.Once);
        }

        [Test]
        public void Edit_NonExistingId_ReturnsNotFoundResult()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.GetReviewByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Review>(null));

            //Act
            var result = _controller.Edit(RandomGenerator.Integer()).Result as HttpNotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._apiProxyMock.Verify(proxy => proxy.GetReviewByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Edit_ExistingIdValidReviewModel_ReturnsRedirectToRouteResult()
        {
            //Arrange
            var review = _reviewBuilder.WithId().Build();
            _controller._apiProxyMock.Setup(proxy => proxy.PutReviewAsync(It.IsAny<int>(), It.IsAny<Review>()))
                .Returns(Task.FromResult(true));
            _controller._apiProxyMock.Setup(proxy => proxy.GetReviewByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Review>(review));

            //Act
            var result = _controller.Edit(review.Id, review).Result as RedirectToRouteResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
            _controller._apiProxyMock.Verify(proxy => proxy.PutReviewAsync(review.Id, review), Times.Once);
        }

        [Test]
        public void Edit_ExistingIdInvalidReviewModel_ReturnsBadRequestResult()
        {
            //Arrange
            var review = _reviewBuilder.WithId().Build();
            _controller.ModelState.AddModelError("ReviewerName", "Review should have a reviewer");

            //Act
            var result = _controller.Edit(review.Id, review).Result as HttpStatusCodeResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            _controller._apiProxyMock.Verify(proxy => proxy.PutReviewAsync(It.IsAny<int>(), It.IsAny<Review>()), Times.Never);
        }

        [Test]
        public void Edit_NonExistingIdAndReviewModel_ReturnsNotFoundResult()
        {
            //Arrange
            var review = _reviewBuilder.WithId().Build();
            _controller._apiProxyMock.Setup(proxy => proxy.GetReviewByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Review>(null));

            //Act
            var result = _controller.Edit(review.Id, review).Result as HttpNotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._apiProxyMock.Verify(proxy => proxy.GetReviewByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Edit_MismatchBetweenUrlIdAndReviewId_ReturnsBadRequestResult()
        {
            //Arrange
            var review = _reviewBuilder.WithId(10).Build();

            //Act
            var result = _controller.Edit(review.Id + 1, review).Result as HttpStatusCodeResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            _controller._apiProxyMock.Verify(proxy => proxy.GetReviewByIdAsync(It.IsAny<int>()), Times.Never);
            _controller._apiProxyMock.Verify(proxy => proxy.PutReviewAsync(It.IsAny<int>(), It.IsAny<Review>()), Times.Never);

        }

        [Test]
        public void Delete_ExistingId_ReturnsRedirectToRouteResult()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.DeleteReviewAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            //Act
            var result = _controller.Delete(RandomGenerator.Integer()).Result as RedirectToRouteResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
            _controller._apiProxyMock.Verify(proxy => proxy.DeleteReviewAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Delete_NonExistingId_ReturnsNotFound()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.DeleteReviewAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            //Act
            var result = _controller.Delete(RandomGenerator.Integer()).Result as HttpNotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._apiProxyMock.Verify(proxy => proxy.DeleteReviewAsync(It.IsAny<int>()), Times.Once);
        }

        class TestableHomeController : HomeController
        {
            public Mock<IApiProxy> _apiProxyMock { get; }
            Mock<HttpContextBase> _contextMock = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> _requestMock = new Mock<HttpRequestBase>();
            Mock<HttpResponseBase> _responseMock = new Mock<HttpResponseBase>();


            private TestableHomeController(Mock<IApiProxy> _apiProxyMock) : base(_apiProxyMock.Object)
            {
                this._apiProxyMock = _apiProxyMock;
                _contextMock.SetupGet(c => c.Request).Returns(_requestMock.Object);
                _contextMock.SetupGet(c => c.Response).Returns(_responseMock.Object);
                _contextMock.SetupGet(c => c.Request.Cookies).Returns(new HttpCookieCollection());
                _contextMock.SetupGet(c => c.Response.Cookies).Returns(new HttpCookieCollection());
                this.ControllerContext = new ControllerContext(_contextMock.Object, new RouteData(), this);
            }

            public static TestableHomeController CreateInstance()
            {
                Mock<IApiProxy> _apiProxyMock = new Mock<IApiProxy>();
                return new TestableHomeController(_apiProxyMock);
            }
        }
    }
}
