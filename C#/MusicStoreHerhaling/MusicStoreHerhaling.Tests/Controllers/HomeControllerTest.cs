using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicStoreHerhaling;
using MusicStoreHerhaling.Controllers;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace MusicStoreHerhaling.Tests.Controllers
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
        public void Index_ReturnsContentContainingControllerNameAndActionName()
        {
            //Arrange

            //Act
            var result = _controller.Index() as ContentResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, 
                Is.EqualTo($"{_controller.RouteData.Values["controller"]}:" +
                           $"{_controller.RouteData.Values["action"]}"));
        }

        [Test]
        public void About_ReturnsContentContainingControllerNameAndActionName()
        {
            //Arrange

            //Act
            var result = _controller.About() as ContentResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content,
                Is.EqualTo($"{_controller.RouteData.Values["controller"]}:" +
                           $"{_controller.RouteData.Values["action"]}"));
        }

        [Test]
        public void Details_ReturnsContentContainingControllerNameActionNameAndParamName()
        {
            //Arrange
            var id = new Random().Next(1, Int32.MaxValue);

            //Act
            var result = _controller.Details(id) as ContentResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content,
                Is.EqualTo($"{_controller.RouteData.Values["controller"]}:" +
                           $"{_controller.RouteData.Values["action"]}:" +
                           $"{id}"));
        }

        [Test]
        public void Search_Rock_PermanentRedirect()
        {
            //Arrange

            //Act
            var result = _controller.Search("Rock") as RedirectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Permanent, Is.True);
            Assert.That(result.Url, Is.EqualTo(HomeController.ROCK_URL));
        }

        [Test]
        public void Search_Jazz_RedirectToIndexAction()
        {
            //Arrange

            //Act
            var result = _controller.Search("Jazz") as RedirectToRouteResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void __()
        {
            //Arrange

            //Act

            //Assert
        }

        class TestableHomeController : HomeController
        {
            public Mock<HttpServerUtilityBase> _contextMock { get; }

            private TestableHomeController(Mock<HttpServerUtilityBase> _contextMock) : base(_contextMock.Object)
            {
                this._contextMock = _contextMock;
            }

            public static TestableHomeController CreateInstance()
            {
                Mock<HttpServerUtilityBase> _contextMock = new Mock<HttpServerUtilityBase>();
                return new TestableHomeController(_contextMock).CreateSomeData();
            }

            private TestableHomeController CreateSomeData()
            {
                var data = new RouteData();

                data.Values["controller"] = Guid.NewGuid().ToString();
                data.Values["action"] = Guid.NewGuid().ToString();

                ControllerContext = new ControllerContext();
                ControllerContext.RouteData = data;
                return this;
            }


        }
    }
}
