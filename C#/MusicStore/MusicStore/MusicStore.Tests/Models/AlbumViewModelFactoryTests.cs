using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Data;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MusicStore.Tests.Models
{
    [TestFixture]
    class AlbumViewModelFactoryTests
    {
        private IAlbumViewModelFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new AlbumViewModelFactory();
        }

        [Test]
        public void Create_ValidAlbumAndValidGenre_CorrectlyMapped()
        {
            //Arrange
            var genre = new GenreBuilder().Build();
            var album = new AlbumBuilder().WithGenreId(genre.Id).Build();

            //Act
            var resultModel = _factory.Create(album, genre) as AlbumViewModel;

            //Assert
            Assert.That(resultModel, Is.Not.Null);
            Assert.That(resultModel.Genre, Is.EqualTo(genre.Name));
            Assert.That(resultModel.Title, Is.EqualTo(album.Title));
            Assert.That(resultModel.Artist, Is.EqualTo(album.Artist));

        }

        [Test]
        public void Create_MissingGenre_ThrowsException()
        {
            //Arrange
            var album = new AlbumBuilder().Build();

            //Act

            //Assert
            Assert.That(() => _factory.Create(album, null),
    Throws.TypeOf<ArgumentException>()
        .With.Message.EqualTo("Illegal parameter : Genre"));
        }

        [Test]
        public void Create_MissingAlbum_TrowsException()
        {
            //Arrange
            var genre = new GenreBuilder().Build();

            //Act

            //Assert
            Assert.That(() => _factory.Create(null, genre),
   Throws.TypeOf<ArgumentException>()
       .With.Message.EqualTo("Illegal parameter : Album"));
        }

        [Test]
        public void Create_MismatchBetweenAlbumAndGenre_TrowsException()
        {
            //Arrange
            Random random = new Random();
            var genre = new GenreBuilder().WithId(random.Next(1, int.MaxValue)).Build();
            var album = new AlbumBuilder().WithGenreId(random.Next(1, int.MaxValue)).Build();

            //Act

            //Assert
            Assert.That(() => _factory.Create(album, genre),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Genre ID mismatch"));
        }
    }
}
