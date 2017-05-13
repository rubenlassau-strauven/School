using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Data.DomainClasses;

namespace MusicStore.Data
{
    public class AlbumViewModelFactory : IAlbumViewModelFactory
    {
        public AlbumViewModel Create(Album album, Genre genre)
        {
            if (genre == null)
                throw new ArgumentException("Illegal parameter : Genre");
            if (album == null)
                throw new ArgumentException("Illegal parameter : Album");
            if (genre.Id != album.GenreId)
                throw new ArgumentException("Genre ID mismatch");
            var model = new AlbumViewModel
            {
                Genre = genre.Name,
                Title = album.Title,
                Artist = album.Artist
            };
            return model;
        }
    }
}
