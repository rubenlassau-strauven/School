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
        public void Create_ValidReview_ReturnsCreatedResult()
        {
            //Arrange
            var review = new ReviewBuilder().Build();
            _controller._apiProxyMock.Setup(proxy => proxy.PostReviewAsync(review))
                .Returns(Task.FromResult<bool>(true));

            //Act
            //var actionResult = _controller.Create(review).Result as Http

            //Assert
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
