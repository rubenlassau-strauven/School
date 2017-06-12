using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicStoreHerhaling.Startup))]
namespace MusicStoreHerhaling
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
