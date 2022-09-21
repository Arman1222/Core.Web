using System.Web.Mvc;
using System.Web.Routing;

namespace MyWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Templates",
                url: "{feature}/Template/{name}",
                defaults: new { controller = "Template", action = "Render" }
                );

            routes.MapRoute(
                name: "DeleteMenuFromRole",
                url: "MenuRole/DeleteMenuFromRole/{menuId}/{roleId}",
                defaults: new { controller = "MenuRole", action = "DeleteMenuFromRole" } 
            );

            routes.MapRoute(
                name: "Paging",
                url: "{controller}/{action}/{pageNumber}/{pageSize}",
                defaults: new { controller = "Home", action = "Index" }
            );
          
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
