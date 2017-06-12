using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.UI.WebControls;
using Moq;
using OdeToFood.Api.Controllers;
using OdeToFood.Api.Tests.TestServices;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Tests.Controllers
{
    [TestFixture]
    class RestaurantsControllerTests
    {
        private TestableRestaurantsController _controller;
        private RestaurantBuilder _builder = new RestaurantBuilder();
        private Random _random = new Random();

        [SetUp]
        public void SetUp()
        {
            _controller = TestableRestaurantsController.CreateInstance();
        }

        [Test]
        public void Get_ReturnsAllRestaurantsFromRepository()
        {
            //Arrange
            List<Restaurant> restaurants = _builder.BuildList(5);
            _controller._repositoryMock.Setup(repo => repo.GetAll()).Returns(restaurants);

            //Act
            var result = _controller.Get() as OkNegotiatedContentResult<IEnumerable<Restaurant>>;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EquivalentTo(restaurants));
            _controller._repositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public void Get_ReturnsRestaurantIfItExists()
        {
            //Arrange
            var restaurant = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.Get(restaurant.Id)).Returns(restaurant);

            //Act
            var result = _controller.Get(restaurant.Id) as OkNegotiatedContentResult<Restaurant>;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo(restaurant));
            _controller._repositoryMock.Verify(repo => repo.Get(restaurant.Id), Times.Once);
        }

        [Test]
        public void Get_ReturnsNotFoundIfItDoesNotExists()
        {
            //Arrange
            _controller._repositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns<Restaurant>(null);

            //Act
            var result = _controller.Get(_random.Next(1, Int32.MaxValue)) as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.Get(It.IsAny<int>()),Times.Once);
        }

        [Test]
        public void Post_ValidRestaurantIsSavedInRepository()
        {
            //Arrange
            var restaurant = _builder.Build();
            _controller._repositoryMock.Setup(repo => repo.Post(restaurant)).Returns(() =>
            {
                restaurant.Id = _random.Next(1, int.MaxValue);
                return restaurant;
            });

            //Act
            var result = _controller.Post(restaurant) as CreatedAtRouteNegotiatedContentResult<Restaurant>;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content.Id, Is.GreaterThanOrEqualTo(0));
            Assert.That(result.Content.City, Is.EqualTo(restaurant.City));
            Assert.That(result.Content.Country, Is.EqualTo(restaurant.Country));
            Assert.That(result.Content.Name, Is.EqualTo(restaurant.Name));
            Assert.That(result.RouteName, Is.EqualTo("DefaultApi"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Restaurants"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(restaurant.Id));
            _controller._repositoryMock.Verify(repo => repo.Post(restaurant), Times.Once);
        }

        [Test]
        public void Post_InValidRestaurantModelStateCausesBadRequest()
        {
            //Arrange
            var restaurant = _builder.WithoutName().Build();
            _controller.ModelState.AddModelError("Name", "Name is required");

            //Act
            var result = _controller.Post(restaurant) as BadRequestResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.Post(It.IsAny<Restaurant>()), Times.Never);
        }

        [Test]
        public void Put_ExistingRestaurantIsSavedInRepository()
        {
            //Arrange
            var restaurant = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.Put(restaurant)).Returns(restaurant);
            _controller._repositoryMock.Setup(repo => repo.Get(restaurant.Id)).Returns(restaurant);

            //Act
            var result = _controller.Put(restaurant.Id, restaurant) as OkResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.Put(restaurant), Times.Once);
        }

        [Test]
        public void Put_NonExistingRestaurantReturnsNotFound()
        {
            //Arrange
            var restaurant = _builder.Build();
            _controller._repositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns<Restaurant>(null);

            //Act
            var result = _controller.Put(restaurant.Id, restaurant) as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once);
            _controller._repositoryMock.Verify(repo => repo.Put(It.IsAny<Restaurant>()), Times.Never);
        }

        [Test]
        public void Put_InValidRestaurantModelStateCausesBadRequest()
        {
            //Arrange
            var restaurant = _builder.WithId().Build();
            _controller.ModelState.AddModelError("Name", "Restaurant should have a name");

            //Act
            var result = _controller.Put(restaurant.Id, restaurant) as BadRequestResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.Get(It.IsAny<int>()), Times.Never);
            _controller._repositoryMock.Verify(repo => repo.Put(It.IsAny<Restaurant>()), Times.Never);
        }

        [Test]
        public void Put_MismatchBetweenUrlIdAndRestaurantIdCausesBadRequest()
        {
            //Arrange
            var restaurant = _builder.WithId(10).Build();

            //Act
            var result = _controller.Put(restaurant.Id + 1, restaurant) as BadRequestResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.Get(It.IsAny<int>()), Times.Never);
            _controller._repositoryMock.Verify(repo => repo.Put(It.IsAny<Restaurant>()), Times.Never);
        }

        [Test]
        public void Delete_ExistingRestaurantIsDeletedFromRepository()
        {
            //Arrange
            var restaurant = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns(restaurant);

            //Act
            var result = _controller.Delete(restaurant.Id) as OkResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.Delete(It.IsAny<int>()),Times.Once);
            _controller._repositoryMock.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Delete_NonExistingRestaurantReturnsNotFound()
        {
            //Arrange
            var restaurant = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns<Restaurant>(null);

            //Act
            var result = _controller.Delete(restaurant.Id) as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once);
            _controller._repositoryMock.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Never);
        }

        class TestableRestaurantsController : RestaurantsController
        {
            public Mock<IRestaurantRepository> _repositoryMock { get; }

            private TestableRestaurantsController(Mock<IRestaurantRepository> _repositoryMock) : base(_repositoryMock.Object)
            {
                this._repositoryMock = _repositoryMock;
            }

            public static TestableRestaurantsController CreateInstance()
            {
                var repo = new Mock<IRestaurantRepository>();
                return new TestableRestaurantsController(repo);
            }
        }
    }
}
