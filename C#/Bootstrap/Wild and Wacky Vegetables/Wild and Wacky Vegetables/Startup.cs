using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Wild_and_Wacky_Vegetables.Startup))]
namespace Wild_and_Wacky_Vegetables
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
