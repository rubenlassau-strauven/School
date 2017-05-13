using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Data;

namespace MusicStore.Tests
{
    public class AlbumViewModelBuilder
    {
        private AlbumViewModel albumViewModel;

        public AlbumViewModelBuilder()
        {
            albumViewModel = new AlbumViewModel
            {
                Genre = Guid.NewGuid().ToString(),
                Title = Guid.NewGuid().ToString(),
                Artist = Guid.NewGuid().ToString()
            };
        }

        public AlbumViewModel Build()
        {
            return albumViewModel;
        }
    }
}
