using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfig.Configure();
            StructureMapConfig.Configure();

            //http://stackoverflow.com/questions/16281366/asp-net-mvc-rendering-seems-slow
            //ViewEngines.Engines.Clear();
            //IViewEngine razorEngine = new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } };
            //ViewEngines.Engines.Add(razorEngine);
        }
        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/adcdd533-d5e3-4af9-b3f5-b9a6d06b5c44/id3206-a-signin-response-may-only-redirect-within-the-current-web-application-url-is-not-allowed?forum=Geneva
        private void Application_BeginRequest(object sender, EventArgs e)
        {
            if (String.Compare(Request.Path, Request.ApplicationPath, StringComparison.InvariantCultureIgnoreCase) == 0
                && !(Request.Path.EndsWith("/")))
                Response.Redirect(Request.Path + "/");
        }
    }
}
