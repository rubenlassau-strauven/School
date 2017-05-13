using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Data.DomainClasses;

namespace MusicStore.Tests
{
    public class AlbumBuilder
    {
        private Album album;
        public AlbumBuilder()
        {
            album = new Album();
            album.Artist = Guid.NewGuid().ToString();
            album.Id = new Random().Next(1, int.MaxValue);
            album.Title = Guid.NewGuid().ToString();
        }

        public Album Build()
        {
           
            return album;
        }

        public AlbumBuilder WithGenreId(int id = -1)
        {
            if (id == -1)
            {
                album.GenreId = new Random().Next(1, Int32.MaxValue);
            }
            else
            {
                album.GenreId = id;
            }
            return this;
        }
    }
}
