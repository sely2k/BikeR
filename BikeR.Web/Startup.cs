using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BikeR.Web.Startup))]
namespace BikeR.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
