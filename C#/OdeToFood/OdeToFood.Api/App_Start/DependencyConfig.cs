using System.Web.Http;
using System.Web.Mvc;
using OdeToFood.Business;
using OdeToFood.Data;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;

namespace OdeToFood.Api
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
            var apiContainer = new Container();
            apiContainer.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            apiContainer.Register<OdeToFoodContext>(() => new OdeToFoodContext(), Lifestyle.Scoped);
            apiContainer.Register<IRestaurantRepository, RestaurantDbRepository>(Lifestyle.Scoped);
            apiContainer.Register<IReviewRepository, ReviewDBRespository>(Lifestyle.Scoped);
            apiContainer.Verify();
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(apiContainer);
        }

        private static void RegisterMvcDependencies()
        {
            var mvcContainer = new Container();
            mvcContainer.Register<IApiProxy>(() => new ApiProxy("http://localhost:60968"));
            mvcContainer.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(mvcContainer));
        }
    }
}