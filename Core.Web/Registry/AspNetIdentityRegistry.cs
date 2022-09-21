using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using MyWeb.Models;
using StructureMap.Configuration.DSL;
using System.Data.Entity;
using System.Web;

namespace Core.Web.Registries
{
	public class AspNetIdentityRegistry : Registry
	{
		public AspNetIdentityRegistry()
		{
            For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
            For<IRoleStore<ApplicationRole, string>>().Use<RoleStore<ApplicationRole>>();
			For<DbContext>().Use<ApplicationDbContext>();
			For<IAuthenticationManager>().Use(ctx => ctx.GetInstance<HttpRequestBase>().GetOwinContext().Authentication);
            For<ICurrentUser>().Use<CurrentUser>();
		}
	}
}