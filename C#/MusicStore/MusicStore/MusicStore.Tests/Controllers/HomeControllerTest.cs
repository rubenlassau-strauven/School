using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicStore;
using MusicStore.Controllers;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace MusicStore.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        #region Old
        //[TestMethod]
        //public void Index()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void About()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.About() as ViewResult;

        //    // Assert
        //    Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        //}

        //[TestMethod]
        //public void Contact()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Contact() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}
        #endregion

        private TestableHomeController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = TestableHomeController.CreateInstance();
        }

        [Test]
        public void Index_ReturnsContentContainingControllerNameAndActionName()
        {
            //Act
            var result = _controller.Index() as ContentResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo($"{_controller.RouteControllerName}:{_controller.RouteActionName}"));
        }

        [Test]
        public void About_ReturnsContentContainingControllerNameAndActionName()
        {
            //Act
            var result = _controller.About() as ContentResult;

            //Assert
            Assert.That(result,Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo($"{_controller.RouteControllerName}:{_controller.RouteActionName}"));
        }

        [Test]
        public void Details_ReturnsContentContainingControllerNameActionNameAndParamName()
        {
            //Act
            var id = new Random().Next();
            var result = _controller.Details(id) as ContentResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo($"{_controller.RouteControllerName}:{_controller.RouteActionName}:{id}"));
        }

        [Test]
        public void Search_Rock_PermanentRedirect()
        {
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
            //Act
            var result = _controller.Search("Jazz") as RedirectToRouteResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Permanent, Is.False);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void Search_Metal_RedirectToDetailsActionWithARandomId()
        {
            //Act
            var result = _controller.Search("Metal") as RedirectToRouteResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Permanent, Is.False);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Details"));
            Assert.That(result.RouteValues["id"], Is.GreaterThan(0));
        }

        [Test]
        public void Search_Classic_ContentOfSiteCssFile()
        {
            //Arrange
            _controller._serverUtilityMock.Setup(su => su.MapPath("~/Content/Site.css"))
               .Returns("C:/Users/11501537/Desktop/School/2e Jaar/Semester 2/Programming Advanced/C#/workspace/MVC/MusicStore/MusicStore/Content/Site.css");

            //Act
            var result = _controller.Search("Classic") as FileContentResult;
           

            //Assert
            Assert.That(result, Is.Not.Null);
            //Assert.That(result.FileDownloadName, Is.EqualTo("Site.css"));
            Assert.That(result.FileContents.Length, Is.GreaterThan(0));
        }

        [Test]
        public void Search_UnknownGenre_ReturnsContentContainingControllerNameActionNameAndGenreParam()
        {
            //Arrange
            var genre = Guid.NewGuid().ToString();

            //Act
            var result = _controller.Search(genre) as ContentResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo($"{_controller.RouteControllerName}:{_controller.RouteActionName}:{genre}"));
        }

        private class TestableHomeController : HomeController
        {
            public string RouteControllerName { get; set; }
            public string RouteActionName { get; set; }
            public Mock<HttpServerUtilityBase> _serverUtilityMock { get; }

            private TestableHomeController(Mock<HttpServerUtilityBase> serverUtilityMock)
                : base(serverUtilityMock.Object)
            {
                _serverUtilityMock = serverUtilityMock;
            }

            public static TestableHomeController CreateInstance()
            {
                return new TestableHomeController(new Mock<HttpServerUtilityBase>()).CreateSomeData();
            }

            public TestableHomeController CreateSomeData()
            {      
                var routeData = new RouteData();
                         
                RouteControllerName = Guid.NewGuid().ToString();
                routeData.Values["controller"] = RouteControllerName;

                RouteActionName = Guid.NewGuid().ToString();
                routeData.Values["action"] = RouteActionName;

                ControllerContext = new ControllerContext();
                ControllerContext.RouteData = routeData;

                return this;
            }
        }
    }
}
