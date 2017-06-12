using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using OdeToFood.Api.Controllers;
using OdeToFood.Api.Tests.TestServices;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Tests.Controllers
{
    [TestFixture]
    class ReviewsControllerTests
    {
        private TestableReviewsController _controller;
        private ReviewBuilder _builder = new ReviewBuilder();

        [SetUp]
        public void SetUp()
        {
            _controller = TestableReviewsController.CreateInstance();
        }

        [Test]
        public void Get_ReturnsAllReviews()
        {
            //Arrange
            List<Review> reviews = _builder.BuildList(5);
            _controller._repositoryMock.Setup(repo => repo.GetAllAsync()).Returns(Task.FromResult(reviews));

            //Act
            var result = _controller.Get().Result as OkNegotiatedContentResult<List<Review>>;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EquivalentTo(reviews));
            _controller._repositoryMock.Verify(repo => repo.GetAllAsync(),Times.Once);
        }

        [Test]
        public void Get_ExistingId_ReturnsReview()
        {
            //Arrange
            var review = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(review));

            //Act
            var result = _controller.Get(review.Id).Result as OkNegotiatedContentResult<Review>;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo(review));
            _controller._repositoryMock.Verify(repo => repo.GetAsync(review.Id), Times.Once);
        }

        [Test]
        public void Get_NonExistingId_ReturnsNotFound()
        {
            //Arrange
            _controller._repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<Review>(null));

            //Act
            var result = _controller.Get(RandomGenerator.Integer()).Result as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Post_ValidModel_ReturnsCreatedAtRoute()
        {
            //Arrange
            var review = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.PostAsync(It.IsAny<Review>())).Returns(Task.FromResult(review));

            //Act
            var result = _controller.Post(review).Result as CreatedAtRouteNegotiatedContentResult<Review>;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo("DefaultApi"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Reviews"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(review.Id));
            _controller._repositoryMock.Verify(repo => repo.PostAsync(review), Times.Once);
        }

        [Test]
        public void Post_InvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller.ModelState.AddModelError("ReviewerName", "Review must have a reviewer");

            //Act
            var result = _controller.Post(_builder.Build()).Result as BadRequestResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.PostAsync(It.IsAny<Review>()), Times.Never);
        }

        [Test]
        public void Put_ExistingIdAndValidModel_ReturnsOkResult()
        {
            //Arrange
            var review = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.PutAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult(review));
            _controller._repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(review));

            //Act
            var result = _controller.Put(review.Id, review).Result as OkResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.PutAsync(review), Times.Once);
            _controller._repositoryMock.Verify(repo => repo.GetAsync(review.Id), Times.Once);
        }

        [Test]
        public void Put_ExistingIdAndInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            var review = _builder.WithId().Build();
            _controller.ModelState.AddModelError("ReviewerName","Review should have a reviewer");

            //Act
            var result = _controller.Put(review.Id, review).Result as BadRequestResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.PutAsync(It.IsAny<Review>()),Times.Never);
        }

        [Test]
        public void Put_NonExistingIdAndValidModel_ReturnsNotFound()
        {
            //Arrange
            var review = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Review>(null));

            //Act
            var result = _controller.Put(RandomGenerator.Integer(), review).Result as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()),Times.Once);
            _controller._repositoryMock.Verify(repo => repo.PutAsync(It.IsAny<Review>()), Times.Never);
        }

        [Test]
        public void Put_MismatchBetweenURLIdAndReviewId_ReturnsBadRequest()
        {
            //Arrange
            var review = _builder.WithId().Build();

            //Act
            var result = _controller.Put(review.Id + 1, review).Result as BadRequestResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.PutAsync(It.IsAny<Review>()), Times.Never);
        }

        [Test]
        public void Delete_ExistingId_ReturnsOkResult()
        {
            //Arrange
            var review = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(review));

            //Act
            var result = _controller.Delete(review.Id).Result as OkResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.GetAsync(review.Id), Times.Once);
            _controller._repositoryMock.Verify(repo => repo.DeleteAsync(review.Id), Times.Once);
        }

        [Test]
        public void Delete_NonExistingId_ReturnsNotFoundResult()
        {
            //Arrange
            var review = _builder.WithId().Build();
            _controller._repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<Review>(null));

            //Act
            var result = _controller.Delete(review.Id).Result as NotFoundResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _controller._repositoryMock.Verify(repo => repo.GetAsync(review.Id), Times.Once);
            _controller._repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        class TestableReviewsController : ReviewsController
        {
            public Mock<IReviewRepository> _repositoryMock { get; }

            private TestableReviewsController(Mock<IReviewRepository> _repositoryMock) : base(_repositoryMock.Object)
            {
                this._repositoryMock = _repositoryMock;
            }

            public static TestableReviewsController CreateInstance()
            {
                var repositoryMock = new Mock<IReviewRepository>();
                return new TestableReviewsController(repositoryMock);
            }
        }
    }
}
