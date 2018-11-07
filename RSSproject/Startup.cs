using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RSSproject.Startup))]
namespace RSSproject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
