using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotNetShopping.Startup))]
namespace DotNetShopping
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
