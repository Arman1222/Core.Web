using Microsoft.Owin.Security;
using System.Linq;
using System.Security.Claims;
using System.Web.Security;

namespace Core.Web.Utilities
{
    //http://www.alexboyang.com/2014/05/28/sso-for-asp-net-mvc4-and-mvc5-web-apps-shared-the-same-domain/
    //SSO for ASP.NET MVC4 and MVC5 web apps shared the same domain
    public class MyTicketDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        public string Protect(AuthenticationTicket data)
        {
            //http://forums.asp.net/t/2059892.aspx?Attaching+Claims+to+the+FormsIdentity+in+ASP+NET+MVC
            //Attaching Claims to the FormsIdentity in ASP.NET MVC
            
            //http://martinwilley.com/blog/2014/03/07/FormsAuthenticationWithClaims.aspx
            //var authTicket = new FormsAuthenticationTicket(
            //      1, //version
            //      data.Identity.Name,
            //      DateTime.Now, //issue date
            //      DateTime.Now.AddMinutes(30), //expiration
            //      false,  //isPersistent
            //      "",
            //      FormsAuthentication.FormsCookiePath); //cookie path
           
            //Create a new Forms Authentication Ticket
            //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(data.Identity.Name, false, -1);
                      

            return FormsAuthentication.Encrypt(new FormsAuthenticationTicket(data.Identity.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier).Value, false, -1));
            
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(protectedText);
            FormsIdentity identity = new FormsIdentity(ticket);
           
            return new AuthenticationTicket(identity, new AuthenticationProperties());
        }        

    }
}
