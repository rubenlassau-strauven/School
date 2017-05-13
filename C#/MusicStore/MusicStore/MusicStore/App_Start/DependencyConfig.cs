using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Data;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace MusicStore.App_Start
{
    public class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            container.Register<HttpServerUtilityBase>(
                () => new HttpServerUtilityWrapper(HttpContext.Current.Server));
            container.Register<IGenreRepository,GenreDummyRepository>(Lifestyle.Scoped);
            container.Register<IAlbumRepository, AlbumDummyRepository>(Lifestyle.Scoped);
            container.Register<IAlbumViewModelFactory, AlbumViewModelFactory>(Lifestyle.Singleton);
            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}