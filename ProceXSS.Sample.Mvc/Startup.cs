using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProceXSS.Sample.Mvc.Startup))]
namespace ProceXSS.Sample.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
