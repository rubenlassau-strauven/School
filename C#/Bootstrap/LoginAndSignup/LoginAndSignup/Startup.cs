using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoginAndSignup.Startup))]
namespace LoginAndSignup
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
