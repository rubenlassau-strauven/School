using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using OdeToFood.Api.Controllers;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;
using NUnit.Framework;

namespace OdeToFood.Api.Tests.Controllers
{
    class ReviewControllerTests
    {
        private TestableReviewController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = TestableReviewController.CreateInstance();
        }

        [Test]
        public void Get_ReturnsAllReviewsFromRepository()
        {
            //Arrange
            var reviews = new List<Review>
            {
                new ReviewBuilder().Build(),
                new ReviewBuilder().Build()
            };
            _controller._reviewRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(Task.FromResult<IEnumerable<Review>>(reviews));

            //Act
            var result = _controller.Get().Result as OkNegotiatedContentResult<IEnumerable<Review>>;

            //Assert
            Assert.That(result,Is.Not.Null);
            Assert.That(result.Content,Is.EquivalentTo(reviews));
        }

        [Test]
        public void Get_ReturnsReviewIfItExists()
        {
            //Arrange
            var review = new ReviewBuilder().Build();
            _controller._reviewRepositoryMock.Setup(rm => rm.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Review>(review));

            //Act
            var result = _controller.Get(review.Id).Result as OkNegotiatedContentResult<Review>;

            //Assert
            _controller._reviewRepositoryMock.Verify(rm => rm.GetAsync(review.Id),Times.Once);
            Assert.That(result,Is.Not.Null);
            Assert.That(result.Content,Is.EqualTo(review));
        }

        [Test]
        public void Get_ReturnsNotFoundIfItDoesNotExists()
        {
            //Arrange
            _controller._reviewRepositoryMock.Setup(rm => rm.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<Review>(null));

            //Act
            var result = _controller.Get(new Random().Next(1, int.MaxValue)).Result as NotFoundResult;

            //Assert
            Assert.That(result,Is.Not.Null);
        }

        [Test]
        public void Post_ValidReviewIsSavedInRepository()
        {
            //Arrange
            var review = new ReviewBuilder().Build();
            _controller._reviewRepositoryMock.Setup(rm => rm.AddAsync(It.IsAny<Review>())).Returns(() =>
            {
                review.Id = new Random().Next(1, int.MaxValue);
                return Task.FromResult<Review>(review);
            });

            //Act
            var result = _controller.Post(review).Result as CreatedAtRouteNegotiatedContentResult<Review>;

            //Assert
            Assert.That(result,Is.Not.Null);
            Assert.That(result.Content.Id, Is.EqualTo(review.Id));
            Assert.That(result.Content.ReviewerName,Is.EqualTo(review.ReviewerName));
            Assert.That(result.RouteName,Is.EqualTo("DefaultApi"));
            Assert.That(result.RouteValues.Count, Is.EqualTo(2));
            Assert.That(result.RouteValues["controller"],Is.EqualTo("Review"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(result.Content.Id));

        }

        [Test]
        public void Post_InValidReviewModelStateCausesBadRequest()
        {
            //Arrange
            _controller.ModelState.AddModelError("ReviewerName","Reviewer name is required");

            //Act
            var result = _controller.Post(new ReviewBuilder().WithoutName().Build()).Result as BadRequestResult;

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Put_ExistingReviewIsSavedInRepository()
        {
            //Arrange
            var review = new ReviewBuilder().WithId().Build();
            _controller._reviewRepositoryMock.Setup(rm => rm.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<Review>(review));
            _controller._reviewRepositoryMock.Setup(rm => rm.UpdateAync(It.IsAny<Review>()))
                .Returns(Task.FromResult<Review>(review));

            //Act
            var result = _controller.Put(review.Id, review).Result as OkResult;

            //Assert
            Assert.That(result,Is.Not.Null);
            _controller._reviewRepositoryMock.Verify(rm => rm.GetAsync(review.Id),Times.Once);
            _controller._reviewRepositoryMock.Verify(rm => rm.UpdateAync(review), Times.Once);
        }

        [Test]
        public void Put_NonExistingReviewReturnsNotFound()
        {
            //Arrange 
            _controller._reviewRepositoryMock.Setup(rm => rm.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<Review>(null));
            var review = new ReviewBuilder().WithId().Build();

            //Act
            var result =_controller.Put(review.Id, review).Result as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Put_InValidReviewModelStateCausesBadRequest()
        {
            //Arrange
            _controller.ModelState.AddModelError("ReviewerName","Reviewer name is required");
            var review = new ReviewBuilder().WithId().WithoutName().Build();

            //Act
            var result = _controller.Put(review.Id, review).Result as BadRequestResult;

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Put_MismatchBetweenUrlIdAndReviewIdCausesBadRequest()
        {
            //Arrange
            var review = new ReviewBuilder().WithId(10).Build();

            //Act
            var result = _controller.Put(20, review).Result as BadRequestResult;

            //Assert
            Assert.That(result,Is.Not.Null);
        }

        [Test]
        public void Delete_ExistingReviewIsDeletedFromRepository()
        {
            //Arrange
            var review = new ReviewBuilder().WithId().Build();
            _controller._reviewRepositoryMock.Setup(rm => rm.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<Review>(review));

            //Act
            var result = _controller.Delete(review.Id).Result as OkResult;

            //Assert
            _controller._reviewRepositoryMock.Verify(rm => rm.GetAsync(review.Id), Times.Once);
            _controller._reviewRepositoryMock.Verify(rm => rm.DeleteAsync(review.Id),Times.Once);
            Assert.That(result,Is.Not.Null);
        }

        [Test]
        public void Delete_NonExistingReviewReturnsNotFound()
        {
            //Arrange
            _controller._reviewRepositoryMock.Setup(rm => rm.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Review>(null));
            var review = new ReviewBuilder().WithId().Build();
            //Act
            var result = _controller.Delete(review.Id).Result as NotFoundResult;

            //Assert
            _controller._reviewRepositoryMock.Verify(rm => rm.GetAsync(review.Id), Times.Once);
            _controller._reviewRepositoryMock.Verify(rm => rm.DeleteAsync(review.Id), Times.Never);
            Assert.That(result, Is.Not.Null);
        }

        private class ReviewBuilder
        {
            private Review review;
            private Restaurant restaurant;

            public ReviewBuilder()
            {
                restaurant = new Restaurant
                {
                    Id = new Random().Next(1, int.MaxValue),
                    Name = Guid.NewGuid().ToString(),
                    Country = Guid.NewGuid().ToString(),
                    City = Guid.NewGuid().ToString()
                };
                review = new Review
                {
                    Body = Guid.NewGuid().ToString(),
                    Rating = new Random().Next(1, 11),
                    RestaurantId = restaurant.Id,
                    Restaurant = restaurant,
                    ReviewerName = Guid.NewGuid().ToString()
                };
            }

            public Review Build()
            {
                return review;
            }

            public ReviewBuilder WithId(int id = 0)
            {
                if (id == 0)
                {
                    review.Id = new Random().Next(1, int.MaxValue);
                }
                else
                {
                    review.Id = id;
                }
                return this;
            }

            public ReviewBuilder WithoutName()
            {
                review.ReviewerName = null;
                return this;
            }
        }

        private class TestableReviewController : ReviewController
        {
            public Mock<IReviewRepository> _reviewRepositoryMock { get; }

            private TestableReviewController(Mock<IReviewRepository> _repositoryMock) : base (_repositoryMock.Object)
            {
                _reviewRepositoryMock = _repositoryMock;
            }

            public static TestableReviewController CreateInstance()
            {
                var reviewRepositoryMock = new Mock<IReviewRepository>();
                return new TestableReviewController(reviewRepositoryMock);
            }
        }
    }
}
