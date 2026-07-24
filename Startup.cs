using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ScrubCRM.Startup))]

namespace ScrubCRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
