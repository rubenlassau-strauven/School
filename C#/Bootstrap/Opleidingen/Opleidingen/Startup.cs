using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Opleidingen.Startup))]
namespace Opleidingen
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
