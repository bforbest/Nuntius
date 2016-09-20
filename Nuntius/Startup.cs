using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Nuntius.Startup))]
namespace Nuntius
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
