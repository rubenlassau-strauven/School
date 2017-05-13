using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MusicStore.Controllers;
using MusicStore.Data;

namespace MusicStore.Tests
{
    public class TestableStoreController : StoreController
    {
        public Mock<IGenreRepository> _genreRepositoryMock { get; }
        public Mock<IAlbumRepository> _albumRepositoryMock { get; }
        public Mock<IAlbumViewModelFactory> _albumViewModelFactoryMock { get; }

        private TestableStoreController(Mock<IGenreRepository> _genreRepositoryMock,
            Mock<IAlbumRepository> _albumRepositoryMock,
            Mock<IAlbumViewModelFactory> _albumViewModelFactoryMock) :
            base(_genreRepositoryMock.Object, _albumRepositoryMock.Object, _albumViewModelFactoryMock.Object)
        {
            this._genreRepositoryMock = _genreRepositoryMock;
            this._albumRepositoryMock = _albumRepositoryMock;
            this._albumViewModelFactoryMock = _albumViewModelFactoryMock;
        }

        public static TestableStoreController CreateInstance()
        {
            var _genreRepositoryMock = new Mock<IGenreRepository>();
            var _albumRepositoryMock = new Mock<IAlbumRepository>();
            var _albumViewModelFactoryMock = new Mock<IAlbumViewModelFactory>();
            return new TestableStoreController(_genreRepositoryMock, _albumRepositoryMock, _albumViewModelFactoryMock);
        }
    }
}
