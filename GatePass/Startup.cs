using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GatePass.Startup))]
namespace GatePass
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
