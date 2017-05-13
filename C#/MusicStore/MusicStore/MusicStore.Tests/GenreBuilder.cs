using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Data.DomainClasses;

namespace MusicStore.Tests
{
    public class GenreBuilder
    {
        private Genre genre;
        public GenreBuilder()
        {
            genre = new Genre();
        }

        public Genre Build()
        {
            genre.Name = Guid.NewGuid().ToString();
            return genre;
        }

        public GenreBuilder WithId(int id = -1)
        {
            if (id == -1)
            {
                genre.Id = new Random().Next(1, int.MaxValue);
            }
            else
            {
                genre.Id = id;
            }
            return this;
        }
    }
}
