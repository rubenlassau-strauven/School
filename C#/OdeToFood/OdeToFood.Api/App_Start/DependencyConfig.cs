using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using OdeToFood.Business;
using OdeToFood.Data;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace OdeToFood.Api
{
    public class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.Register<OdeToFoodContext>(() => new OdeToFoodContext(), Lifestyle.Scoped);
            container.Register<IRestaurantRepository, RestaurantDbRepository>(Lifestyle.Scoped);
            container.Register<IReviewRepository, ReviewDBRespository>(Lifestyle.Scoped);
            container.Register<IApiProxy>(() => new ApiProxy("http://localhost:60968"), Lifestyle.Singleton);
            var result = container.GetInstance(typeof(IApiProxy));
            container.Verify();
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
        

    }
}