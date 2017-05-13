using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Castle.Core.Internal;
using Moq;
using NUnit.Framework;
using OdeToFood.Api.Controllers;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Tests.Controllers
{
    [TestFixture]
    public class RestaurantsControllerTests
    {
        private TestableRestaurantsController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = TestableRestaurantsController.CreateInstance();
        }

        [Test]
        public void Get_ReturnsAllRestaurantsFromRepository()
        {
            //Arrange
            var allRestaurants = new List<Restaurant>
            {
               new RestaurantBuilder().Build(),
               new RestaurantBuilder().Build()
        };

            _controller._restaurantsRepositoryMock.Setup(repo => repo.GetAll()).Returns(allRestaurants);

            //Act
            var result = _controller.Get() as OkNegotiatedContentResult<IEnumerable<Restaurant>>;

            //Assert
            Assert.That(result,Is.Not.Null);
            Assert.That(result.Content, Is.EquivalentTo(allRestaurants));
        }

        [Test]
        public void Get_ReturnsRestaurantIfItExists()
        {
            //Arrange
            var restaurant = new RestaurantBuilder().WithId().Build();
            _controller._restaurantsRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns(restaurant);

            //Act
            var result = _controller.Get(restaurant.Id) as OkNegotiatedContentResult<Restaurant>;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._restaurantsRepositoryMock.Verify(rm => rm.Get(restaurant.Id), Times.Once);
            Assert.That(result.Content,Is.EqualTo(restaurant));
        }

        [Test]
        public void Get_ReturnsNotFoundIfItDoesNotExists()
        {
            //Arrange
            var restaurant = new RestaurantBuilder().WithId().Build();
            _controller._restaurantsRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns(() => null);    //Rep vindt nooit een resultaat

            //Act
            var result = _controller.Get(new Random().Next(1,int.MaxValue)) as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);

        }

        [Test]
        public void Post_ValidRestaurantIsSavedInRepository()
        {
            //Arrange
            var restaurant = new RestaurantBuilder().Build();
            _controller._restaurantsRepositoryMock.Setup(repo => repo.Add(It.IsAny<Restaurant>())).Returns(() =>
            {
                restaurant.Id = new Random().Next();
                return restaurant;
            });

            //Act
            var result = _controller.Post(restaurant) as CreatedAtRouteNegotiatedContentResult<Restaurant>;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content.Id,Is.GreaterThan(0));
            Assert.That(result.Content.Name, Is.EqualTo(restaurant.Name));
            Assert.That(result.RouteName,Is.EqualTo("DefaultApi"));
            Assert.That(result.RouteValues.Count, Is.EqualTo(2));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Restaurants"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(result.Content.Id));

        }

        [Test]
        public void Post_InValidRestaurantModelStateCausesBadRequest()
        {
            //Arrange
            _controller.ModelState.AddModelError("Name","Name is required");    //Zeg tegen het model dat er iets fout is

            //Act
            var result = _controller.Post(new RestaurantBuilder().WithoutName().Build()) as BadRequestResult;   //Without name is hier eigenlijk overbodig

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Put_ExistingRestaurantIsSavedInRepository()
        {
            //Arrange
            var restaurant = new RestaurantBuilder().WithId().Build();
            _controller._restaurantsRepositoryMock.Setup(rm => rm.Get(It.IsAny<int>())).Returns(restaurant);
            _controller._restaurantsRepositoryMock.Setup(rm => rm.Update(It.IsAny<Restaurant>())).Returns(restaurant);

            //Act
            var result = _controller.Put(restaurant.Id, restaurant) as OkResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._restaurantsRepositoryMock.Verify(rm => rm.Get(restaurant.Id),Times.Once);
            _controller._restaurantsRepositoryMock.Verify(rm => rm.Update(It.IsAny<Restaurant>()), Times.Once);
        }

        [Test]
        public void Put_NonExistingRestaurantReturnsNotFound()
        {
            //Arrange
            _controller._restaurantsRepositoryMock.Setup(rm => rm.Get(It.IsAny<int>())).Returns(() => null);
            var restaurant = new RestaurantBuilder().WithId().Build();

            //Act
            var result = _controller.Put(restaurant.Id,restaurant) as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Put_InValidRestaurantModelStateCausesBadRequest()
        {
            //Arrange
            var restaurant = new RestaurantBuilder().WithId().WithoutName().Build();
            _controller.ModelState.AddModelError("Name","Name is required");

            //Act
            var result = _controller.Put(restaurant.Id,restaurant) as BadRequestResult;

            //Assert
            Assert.That(result,Is.Not.Null);
        }

        [Test]
        public void Put_MismatchBetweenUrlIdAndRestaurantIdCausesBadRequest()
        {
            //Arrange
            var restaurant = new RestaurantBuilder().WithId(10).Build();

            //Act
            var result = _controller.Put(20, restaurant) as BadRequestResult;

            //Assert
            Assert.That(result,Is.Not.Null);
        }

        [Test]
        public void Delete_ExistingRestaurantIsDeletedFromRepository()
        {
            //Arrange
            var restaurant = new RestaurantBuilder().WithId().Build();
            _controller._restaurantsRepositoryMock.Setup(rm => rm.Get(It.IsAny<int>())).Returns(restaurant);

            //Act
            var result = _controller.Delete(restaurant.Id) as OkResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._restaurantsRepositoryMock.Verify(r => r.Get(restaurant.Id), Times.Once);
            _controller._restaurantsRepositoryMock.Verify(r => r.Delete(restaurant.Id), Times.Once);
        }

        [Test]
        public void Delete_NonExistingRestaurantReturnsNotFound()
        {
            //Arrange
            var restaurant = new RestaurantBuilder().WithId().Build();
            _controller._restaurantsRepositoryMock.Setup(rm => rm.Get(It.IsAny<int>())).Returns(() => null);

            //Act
            var result = _controller.Delete(restaurant.Id) as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._restaurantsRepositoryMock.Verify(r => r.Get(restaurant.Id), Times.Once);
            _controller._restaurantsRepositoryMock.Verify(r => r.Delete(restaurant.Id), Times.Never);
        }

        private class TestableRestaurantsController : RestaurantsController
        {
            public Mock<IRestaurantRepository> _restaurantsRepositoryMock { get; }

            private TestableRestaurantsController(Mock<IRestaurantRepository> repository) : base(repository.Object)
            {
                _restaurantsRepositoryMock = repository;
            }

            public static TestableRestaurantsController CreateInstance()
            {
                var restaurantsRepositoryMock = new Mock<IRestaurantRepository>();
                return new TestableRestaurantsController(restaurantsRepositoryMock);
            }
        }

        private class RestaurantBuilder
        {
            Restaurant res = new Restaurant
            {
                City = Guid.NewGuid().ToString(),
                Country = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };

            public Restaurant Build()
            {
                return res;
            }

            public RestaurantBuilder WithId(int id = 0)
            {
                if (id == 0)
                {
                    res.Id = new Random().Next();
                }
                else
                {
                    res.Id = id;
                }
                return this;
            }

            public RestaurantBuilder WithoutName()
            {
                res.Name = null;
                return this;
            }
        }

    }
}
