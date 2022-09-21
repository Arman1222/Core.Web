using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Cors;

[assembly: OwinStartupAttribute(typeof(Mayora.Startup))]
namespace Mayora
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            //EnableCors(app);
        }

        private void EnableCors(IAppBuilder app)
        {
            var policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };

            var origins = ConfigurationManager.AppSettings["Cors.AllowedOrigins"];
            if (origins != null)
            {
                foreach (var origin in origins.Split(';'))
                {
                    policy.Origins.Add(origin);
                }
            }
            else
            {
                policy.AllowAnyOrigin = true;
            }

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy)
                }
            };
            app.UseCors(corsOptions);
        }
    }
}
