using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SIEVK.Service.Startup))]
namespace SIEVK.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
