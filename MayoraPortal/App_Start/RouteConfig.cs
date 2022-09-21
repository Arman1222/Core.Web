using Core.Web.Utilities;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mayora
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("AccountRoute", "Account/{action}/{id}",
                    new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                    new[] { "MyWeb.Portal.Controllers" });

            routes.MapRoute(
                name: "Templates",
                url: "{feature}/Template/{name}",
                defaults: new { controller = "Template", action = "Render" }
                );

            routes.MapRoute(
                name: "Paging",
                url: "{controller}/{action}/{pageNumber}/{pageSize}",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = WebHelpers.GetPortalControllerName(), action = WebHelpers.GetPortalActionName(), id = UrlParameter.Optional }
            );
        }
    }
}
