using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using OdeToFood.Api;
using OdeToFood.Api.Controllers;
using OdeToFood.Api.Tests.Utilities;
using OdeToFood.Business;
using OdeToFood.Data.DomainClasses;
using Assert = NUnit.Framework.Assert;

namespace OdeToFood.Api.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        private TestableHomeController _controller;

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
            var viewResult = _controller.About() as ViewResult;

            //Assert
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewBag.LastVisit, Is.EqualTo("Never"));
            Assert.That(_controller._httpCookieCollection["LastVisited"], Is.Not.Null);
            Assert.That(_controller._httpCookieCollection["LastVisited"].Value, Is.Not.Null);
        }

        [Test]
        public void About_SecondVisit_ReturnsViewWithLastVisitedDateAndSavesNewVisitToACookie()
        {
            //Arrange

            //Act
            _controller.About();
            var lastVisit = _controller._httpCookieCollection["LastVisited"].Value;
            var secondVisit = _controller.About() as ViewResult;

            //Assert
            Assert.That(secondVisit, Is.Not.Null);
            Assert.That(secondVisit.ViewBag.LastVisit, Is.EqualTo(lastVisit));
            Assert.That(_controller._httpCookieCollection["LastVisited"], Is.Not.Null);
            Assert.That(_controller._httpCookieCollection["LastVisited"].Value, Is.Not.Null);
            Assert.That(_controller._httpCookieCollection.Count, Is.EqualTo(1));
        }

        [Test]
        public void Index_ReturnsAllReviews()
        {
            //Arrange
            var reviewBuilder = new ReviewBuilder();
            var reviews = new List<Review>
            {
                reviewBuilder.WithId().Build(),
                reviewBuilder.WithId().Build()
            };
            _controller._apiProxyMock.Setup(proxy => proxy.GetReviewsAsync())
                .Returns(Task.FromResult<IEnumerable<Review>>(reviews));

            //Act
            var viewResult = _controller.Index().Result as ViewResult;

            //Assert
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EquivalentTo(reviews));
            _controller._apiProxyMock.Verify(proxy => proxy.GetReviewsAsync(), Times.Once);
        }

        [Test]
        public void RestaurantDetails_ValidId_ReturnsRestaurant()
        {
            //Arrange
            var restaurant = new RestaurantBuilder().WithId().Build();
            _controller._apiProxyMock.Setup(proxy => proxy.GetRestaurantByIdAsync(restaurant.Id))
                .Returns(Task.FromResult<Restaurant>(restaurant));

            //Act
            var viewResult = _controller.RestaurantDetails(restaurant.Id).Result as ViewResult;

            //Assert
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(restaurant));
            _controller._apiProxyMock.Verify(proxy => proxy.GetRestaurantByIdAsync(restaurant.Id), Times.Once);
        }

        [Test]
        public void RestaurantDetails_InvalidId_ReturnsNotFoundResult()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.GetRestaurantByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Restaurant>(null));

            //Act
            var actionResult = _controller.RestaurantDetails(new Random().Next(1, int.MaxValue)).Result as HttpNotFoundResult;

            //Assert
            Assert.That(actionResult, Is.Not.Null);
        }

        [Test]
        public void Create_ReturnsEditView()
        {
            //Arrange

            //Act
            var redirectResult = _controller.Create() as ViewResult;

            //Assert
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.ViewBag.Title, Is.EqualTo("New Review"));
        }

        [Test]
        public void Create_ValidReview_ReturnsRedirectToRouteResult()
        {
            //Arrange
            var review = new ReviewBuilder().Build();
            _controller._apiProxyMock.Setup(proxy => proxy.PostReviewAsync(review))
                .Returns(Task.FromResult<bool>(true));

            //Act
            var redirectResult = _controller.Create(review).Result as RedirectToRouteResult;

            //Assert
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Permanent, Is.False);
            Assert.That(redirectResult.RouteValues["Action"], Is.EqualTo("Index"));
            _controller._apiProxyMock.Verify(proxy => proxy.PostReviewAsync(review), Times.Once);
        }

        [Test]
        public void Create_InvalidReview_ReturnsBadRequest()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.PostReviewAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult<bool>(false));

            //Act
            var actionresult = _controller.Create(new Review()).Result as HttpStatusCodeResult;

            //Assert
            Assert.That(actionresult, Is.Not.Null);
            Assert.That(actionresult.StatusCode, Is.EqualTo(400));
            Assert.That(actionresult.StatusDescription, Is.EqualTo("Bad Request: Invalid review"));
        }

        [Test]
        public void Edit_ValidId_ReturnsEditView()
        {
            //Arrange
            var review = new ReviewBuilder().WithId().Build();
            _controller._apiProxyMock.Setup(proxy => proxy.GetReviewByIdAsync(review.Id)).Returns(Task.FromResult<Review>(review));

            //Act
            var viewResult = _controller.Edit(review.Id).Result as ViewResult;

            //Assert
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(review));
            _controller._apiProxyMock.Verify(proxy => proxy.GetReviewByIdAsync(review.Id), Times.Once);
        }

        [Test]
        public void Edit_InvalidId_ReturnsBadRequest()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.GetReviewByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<Review>(null));

            //Act
            var actionresult = _controller.Edit(new Random().Next(1,int.MaxValue)).Result as HttpStatusCodeResult;

            //Assert
            Assert.That(actionresult, Is.Not.Null);
            Assert.That(actionresult.StatusCode, Is.EqualTo(400));
            Assert.That(actionresult.StatusDescription, Is.EqualTo("Bad Request: Review not found"));
            _controller._apiProxyMock.Verify(proxy => proxy.GetReviewByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Edit_ValidIdAndValidReview_ReturnsRedirectToRouteResult()
        {
            //Arrange
            var review = new ReviewBuilder().WithId().Build();
            _controller._apiProxyMock.Setup(proxy => proxy.PutReviewAsync(review.Id, review))
                .Returns(Task.FromResult<bool>(true));

            //Act
            var redirectResult = _controller.Edit(review.Id, review).Result as RedirectToRouteResult;

            //Assert
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Permanent, Is.False);
            Assert.That(redirectResult.RouteValues["Action"], Is.EqualTo("Index"));
            _controller._apiProxyMock.Verify(proxy => proxy.PutReviewAsync(review.Id, review), Times.Once);
        }

        [Test]
        public void Edit_InvalidIdAndOrInvalidReview_ReturnsBedRequest()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.PutReviewAsync(It.IsAny<int>(), It.IsAny<Review>()))
                .Returns(Task.FromResult<bool>(false));

            //Act
            var actionresult = _controller.Edit(new Random().Next(1,int.MaxValue), new Review()).Result as HttpStatusCodeResult;

            //Assert
            Assert.That(actionresult, Is.Not.Null);
            Assert.That(actionresult.StatusCode, Is.EqualTo(400));
            Assert.That(actionresult.StatusDescription, Is.EqualTo("Bad Request: Invalid review"));
            _controller._apiProxyMock.Verify(proxy => proxy.PutReviewAsync(It.IsAny<int>(), It.IsAny<Review>()), Times.Once);
        }

        [Test]
        public void Delete_ValidId_ReturnsRedirectToRouteResult()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.DeleteReviewAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<bool>(true));

            //Act
            var redirectResult = _controller.Delete(new Random().Next(1, int.MaxValue)).Result as RedirectToRouteResult;

            //Assert
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Permanent, Is.False);
            Assert.That(redirectResult.RouteValues["Action"], Is.EqualTo("Index"));
            _controller._apiProxyMock.Verify(proxy => proxy.DeleteReviewAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void Delete_InvalidId_ReturnsBadRequest()
        {
            //Arrange
            _controller._apiProxyMock.Setup(proxy => proxy.DeleteReviewAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<bool>(false));

            //Act
            var actionresult = _controller.Delete(new Random().Next(1, int.MaxValue)).Result as HttpStatusCodeResult;

            //Assert
            Assert.That(actionresult, Is.Not.Null);
            Assert.That(actionresult.StatusCode, Is.EqualTo(400));
            Assert.That(actionresult.StatusDescription, Is.EqualTo("Bad Request: Review not found"));
            _controller._apiProxyMock.Verify(proxy => proxy.DeleteReviewAsync(It.IsAny<int>()), Times.Once);
        }

        private class TestableHomeController : HomeController
        {
            public Mock<IApiProxy> _apiProxyMock { get; set; }

            private TestableHomeController(Mock<IApiProxy> _ApiProxyMock) : base (_ApiProxyMock.Object)
            {
                this._apiProxyMock = _ApiProxyMock;
            }

            public static TestableHomeController CreateInstance()
            {
                var apiProxyMock = new Mock<IApiProxy>();
                return new TestableHomeController(apiProxyMock);
            }
        }
    }
}
