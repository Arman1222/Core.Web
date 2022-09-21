using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mayora.Startup))]
namespace Mayora
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseCors(CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}
