using System;
using System.Web;

namespace Core.Web.Utilities
{
    public static class WebHelpers
    {
        //http://stackoverflow.com/questions/7413466/how-can-i-get-the-baseurl-of-site
        public static string BaseUrl()
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
        }
        //http://stackoverflow.com/questions/1288046/how-can-i-get-my-webapps-base-url-in-asp-net-mvc
        public static string GetBaseUrl(this HttpRequestBase request)
        {
          if (request.Url == (Uri) null)
            return string.Empty;
          else
            return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
        }
        public static string GetPortalControllerName()
        {
            return "Portal";
        }
        public static string GetPortalActionName()
        {
            return "Index";
        }
    }
}
