using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Data.DomainClasses;

namespace MusicStore.Data
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetAll();
        Genre GetById(int genreId);
    }
}
