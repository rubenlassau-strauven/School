using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Data.DomainClasses;

namespace MusicStore.Data
{
    public class AlbumDummyRepository : IAlbumRepository
    {
        public IEnumerable<Album> GetByGenre(int genreId)
        {
            var albums = new List<Album>();
            var nbrOfAlbums = 3;
            for (int i = 1; i <= nbrOfAlbums; i++)
            {
                albums.Add(new Album
                {
                    Id = i + (nbrOfAlbums * (genreId - 1)),
                    Artist = "Artist " + i,
                    Title = "Title " + i,
                    GenreId = genreId
                });
            }
            return albums;

        }

        public Album GetById(int albumId)
        {
            var genreRepo = new GenreDummyRepository();
            var genre = genreRepo.GetAll().First();

            return new Album
            {
                Id = albumId,
                Artist = "Artist " + albumId,
                Title = "Title " + albumId,
                GenreId = genre.Id
            };

        }
    }
}
