using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using MusicStore.Controllers;
using MusicStore.Data;
using MusicStore.Data.DomainClasses;
using NUnit.Framework;

namespace MusicStore.Tests.Controllers
{
    [TestFixture]
    class StoreControllerTests
    {
        private TestableStoreController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = TestableStoreController.CreateInstance();
        }

        [Test]
        public void Index_ShowsListOfMusicGenres()
        {
            //Arrange
            var allGenres = new List<Genre>
            {
                new GenreBuilder().Build(),
                new GenreBuilder().Build()
            };

            _controller._genreRepositoryMock.Setup(repo => repo.GetAll()).Returns(allGenres);

            //Act
            var viewResult = _controller.Index() as ViewResult;

            //Assert
            Assert.That(viewResult,Is.Not.Null);
            _controller._genreRepositoryMock.Verify(repo => repo.GetAll(),Times.Once);
            Assert.That(viewResult.Model, Is.EquivalentTo(allGenres));
        }

        [Test]
        public void Browse_ShowsAlbumsOfGenre()
        {
            //Arrange
            var albumsOfCertainGenre = new List<Album>
            {
                new AlbumBuilder().Build(),
                new AlbumBuilder().Build()
            };
            _controller._albumRepositoryMock.Setup(repo => repo.GetByGenre(It.IsAny<int>()))
                .Returns(albumsOfCertainGenre);
            _controller._genreRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new GenreBuilder().Build());

            //Act
            var resultView = _controller.Browse(new Random().Next(1, int.MaxValue)) as ViewResult;

            //Assert
            Assert.That(resultView, Is.Not.Null);
            _controller._albumRepositoryMock.Verify(repo => repo.GetByGenre(It.IsAny<int>()), Times.Once);
            Assert.That(resultView.Model, Is.EquivalentTo(albumsOfCertainGenre));
            Assert.That(_controller.ViewBag.Genre, Is.Not.Null);
        }

        [Test]
        public void Browse_InvalidGenreId_ReturnsNotFound()
        {
            //Arrange
            _controller._genreRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns<Genre>(null);

            //Act
            var viewResult = _controller.Browse(new Random().Next(1, int.MaxValue)) as HttpNotFoundResult;

            //Assert
            Assert.That(viewResult, Is.Not.Null);
            _controller._genreRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Browse_ValidGenreIdButNoAlbumsForGenre_ShowsEmptyList()
        {
            //Arrange
            _controller._genreRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new GenreBuilder().Build());

            //Act
            var viewResult = _controller.Browse(new Random().Next(1, int.MaxValue)) as ViewResult;

            //Assert
            Assert.That(viewResult, Is.Not.Null);
            _controller._albumRepositoryMock.Verify(repo => repo.GetByGenre(It.IsAny<int>()), Times.Once);
            Assert.That(viewResult.Model, Is.EquivalentTo(new List<Genre>()));
        }

        [Test]
        public void Details_ShowsDetailsOfAlbum()
        {
            //Arrange
            var genre = new GenreBuilder().WithId().Build();
            var album = new AlbumBuilder().WithGenreId(genre.Id).Build();
            var albumViewModel = new AlbumViewModelBuilder().Build();
            _controller._albumRepositoryMock.Setup(repo => repo.GetById(album.Id))
                .Returns(album);
            _controller._genreRepositoryMock.Setup(repo => repo.GetById(genre.Id)).Returns(genre);
            _controller._albumViewModelFactoryMock.Setup(factory => factory.Create(album, genre))
                .Returns(albumViewModel);

            //Act
            var resultView = _controller.Details(album.Id) as ViewResult;

            //Assert
            Assert.That(resultView, Is.Not.Null);
            _controller._albumRepositoryMock.Verify(repo => repo.GetById(album.Id),Times.Once);
            _controller._genreRepositoryMock.Verify(repo => repo.GetById(album.GenreId), Times.Once);
            _controller._albumViewModelFactoryMock.Verify(factory => factory.Create(album, genre), Times.Once);
            Assert.That(resultView.Model, Is.EqualTo(albumViewModel));
        }

        [Test]
        public void Details_InvalidId_ReturnsNotFound()
        {
            //Arrange
            _controller._albumRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns<Album>(null);

            //Act
            var viewResult = _controller.Details(new Random().Next(1, Int32.MaxValue)) as HttpNotFoundResult;

            //Assert
            Assert.That(viewResult, Is.Not.Null);
        }
    }
}
