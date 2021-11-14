using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(test3.Startup))]
namespace test3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
