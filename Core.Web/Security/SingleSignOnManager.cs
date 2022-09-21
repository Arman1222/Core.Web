using System.Web;

namespace Core.Web.Security
{
    //http://www.wiktorzychla.com/2010/12/wif-ws-federation-and-single-sign-out.html
    public class SingleSignOnManager
    {
        const string SITECOOKIENAME = "StsSiteCookie";
        const string SITENAME       = "StsSite";
 
        /// <summary>
        /// Returns a list of sites the user is logged in via the STS
        /// </summary>
        /// <returns></returns>
        public static string[] SignOut()
        {
            if ( HttpContext.Current != null &&
                 HttpContext.Current.Request != null &&
                 HttpContext.Current.Request.Cookies != null 
                )
            {
                HttpCookie siteCookie = 
                    HttpContext.Current.Request.Cookies[SITECOOKIENAME];
 
                if ( siteCookie != null )
                    return siteCookie.Values.GetValues(SITENAME);
            }
                 
            return new string[0];
        }
 
        public static void RegisterRP( string SiteUrl )
        {
            if ( HttpContext.Current != null &&
                 HttpContext.Current.Request != null &&
                 HttpContext.Current.Request.Cookies != null
                )
            {
                // get an existing cookie or create a new one
                HttpCookie siteCookie = 
                    HttpContext.Current.Request.Cookies[SITECOOKIENAME];
                if ( siteCookie == null )
                    siteCookie = new HttpCookie( SITECOOKIENAME );
 
                siteCookie.Values.Add( SITENAME, SiteUrl );
 
                HttpContext.Current.Response.AppendCookie( siteCookie );
            }
        }
    }
}
