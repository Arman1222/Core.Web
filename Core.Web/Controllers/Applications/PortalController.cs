using Core.Web.Controllers;
using Core.Web.Helpers;
using Core.Web.Models.Applications;
using Core.Web.Security;
using Microsoft.AspNet.Identity;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mayora.Controllers
{    
    [Authorize]
    public class PortalController : CoreControllerBase
    {
        public const string Action = "wa";
        public const string SignIn = "wsignin1.0";
        public const string SignOut = "wsignout1.0";
        public const string SignoutCleanup = "wsignoutcleanup1.0";
        public const string Reply = "wreply";

        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;        
        private SqlHelper _query = new SqlHelper("SqlCoreConnection");

        public PortalController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }
        public async Task<ActionResult> Index()
        {
            var action = Request.QueryString[Action];
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserName = User.Identity.GetUserName();
                if (action == SignIn)
                {                    
                    var formData = ProcessSignIn(Request.Url, (ClaimsPrincipal)User);
                    return new ContentResult() { Content = formData, ContentType = "text/html" };
                }
                else if (action == SignOut || action == SignoutCleanup)
                {
                    var reply = Request.QueryString[Reply];

                    //SignInRequestMessage signInRequestMessage = new SignInRequestMessage(new Uri(Request.GetBaseUrl()), reply, reply);
                    //String queryString = signInRequestMessage.WriteQueryString();

                    return RedirectToAction("LogOff", "Account", new { returnUrl = reply });                    
                }               
            }
            //List<Application> list = await GetApplicationList();
            return View();
            // RedirectToAction("Login", "Account", new { ReturnUrl = Request.Url });
        }

        [AllowAnonymous]
        public async Task<JsonResult> All()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var models = await ctx.ApplicationSet
                            .Where(c => c.IsDelete == false)
                            .OrderBy(c => c.ApplicationName)
                            .ToListAsync();

                    return JsonSuccess(new { data = models.ToArray() });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> MyListApplication()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var models = ctx.Sp_get_application_by_userId(_currentUser.User.Id)
                            .ToList();

                    return JsonSuccess(new { data = models.ToArray() });
                }
            });
        }

        private async Task<List<Application>> GetApplicationList()
        {
            using (var ctx = new ApplicationDbContext())
            {
                    return await ctx.ApplicationSet
                        .Where(c=>c.IsDelete == false)
                        .OrderBy(c=>c.ApplicationName)
                        .ToListAsync();
            }
        }

        //http://chris.59north.com/post/Building-a-simple-custom-STS-using-VS2012-ASPNET-MVC     
        private static string ProcessSignIn(Uri url, ClaimsPrincipal user)
        {
            var requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(url);
            var signingCredentials = new X509SigningCredentials(CustomSecurityTokenService.GetCertificate(ConfigurationManager.AppSettings["SigningCertificateName"]));
            var config = new SecurityTokenServiceConfiguration(ConfigurationManager.AppSettings["IssuerName"], signingCredentials);
            var sts = new CustomSecurityTokenService(config);
            var responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, user, sts);
             //FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(
             //   responseMessage, 
             //   System.Web.HttpContext.Current.Response);
            
            return responseMessage.WriteFormPost();
        }

        [AllowAnonymous]
        //http://www.stevesdevbox.com/Blogs/Writing-an-MVC-Security-Token-Service-for-Development
        public ActionResult ProcessSignOut()
        {

            var message = WSFederationMessage.CreateFromUri(Request.Url);
            string reply = null;

            if (message.GetType() == typeof(SignOutCleanupRequestMessage))
            {
                reply = ((SignOutCleanupRequestMessage)message).Reply;
            }
            else if (message.GetType() == typeof(SignOutRequestMessage))
            {
                reply = ((SignOutRequestMessage)message).Reply;
            }

            FederatedPassiveSecurityTokenServiceOperations.ProcessSignOutRequest(
                message,
                (ClaimsPrincipal)User,
                reply,
                System.Web.HttpContext.Current.Response);
            return Redirect(reply ?? "/");                        
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}