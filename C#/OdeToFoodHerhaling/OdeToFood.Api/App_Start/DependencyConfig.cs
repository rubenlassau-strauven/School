using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using OdeToFood.Business;
using OdeToFood.Data;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Integration.Web.Mvc;

namespace OdeToFood.Api.App_Start
{
    public class DependencyConfig
    {
        public static void RegisterDependencies()
        {
           RegisterApiDependencies();
           RegisterMvcDependencies();
        }

        private static void RegisterApiDependencies()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.Register<OdeToFoodContext>(() => new OdeToFoodContext(), Lifestyle.Scoped);
            container.Register<IRestaurantRepository, RestaurantDbRepository>(Lifestyle.Scoped);
            container.Register<IReviewRepository, ReviewDbRepository>(Lifestyle.Scoped);
            container.Verify();
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void RegisterMvcDependencies()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            container.Register<IApiProxy>(() => new ApiProxy("http://localhost:61715"));
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}