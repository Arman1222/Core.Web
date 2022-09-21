using Core.Web.ActionResults;
using Core.Web.Models.Applications;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyWeb.DataLayer;
using MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

//http://www.scriptscoop.net/t/1851262c1cc7/asp.net-how-to-use-owin-forms-authentication-without-aspnet-identity.html
namespace Core.Web.Infrastructure
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {        
        public string Roles
        {
            get { return _identityRoles ?? String.Empty; }
            set
            {
                _identityRoles = value;
                _identityRolesSplit = SplitString(value);
            }
        }

        //public string Application
        //{
        //    get { return _application ?? String.Empty; }
        //    set
        //    {
        //        _application = value;              
        //    }
        //}

        private string _identityRoles;
        private string[] _identityRolesSplit = new string[0];
        //private string _application;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //do the base class AuthorizeCore first
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            if (_identityRolesSplit.Length > 0)
            {
                //get the UserManager
                using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                {
                    string userId = string.Empty;
                    if (HttpContext.Current.User.Identity.AuthenticationType == "Forms")
                    {
                        userId = HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        userId = HttpContext.Current.User.Identity.GetUserId();     
                    }

                    string actionName = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                    string controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();

                    //get the Roles for this user
                    var roles = um.GetRoles(userId);

                    if (roles.Contains("Admin")) //if admin true
                    {
                        return true;
                    }
                    //if the at least one of the Roles of the User is in the IdentityRoles list return true
                    else if (_identityRolesSplit.Any(roles.Contains))
                    {
                        using (var _applicationDbContext = new ApplicationDbContext())
                        {
                            var appName = WebConfigurationManager.AppSettings["ApplicationName"];
                            Application app = _applicationDbContext.ApplicationSet.FirstOrDefault(a => a.ApplicationName == appName);

                            List<Navbar> menuList = _applicationDbContext.Menus.Where(m => m.ApplicationId == app.ApplicationId
                                 && m.Controller.ToLower() == controllerName.ToLower()
                                //&& m.Action.ToLower() == actionName.ToLower()
                                 ).ToList();

                            foreach (var menu in menuList)
                            {
                                if (menu.Roles.Any(r => _identityRolesSplit.Contains(r.Name)))
                                {
                                    return true;
                                }
                            }

                            //if (app != null && _applicationDbContext.Menus.Count(m => m.ApplicationId == app.ApplicationId 
                            //    && m.Controller.ToLower() == controllerName.ToLower() 
                            //    && m.Action.ToLower() == actionName.ToLower() 
                            //    && m.Roles.Any(r => roles.Contains(r.Name))) > 0)             
                            //    return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //if the user is not logged in use the deafult HandleUnauthorizedRequest and redirect to the login page
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            //if the user is logged in but is trying to access a page he/she doesn't have the right for show the access denied page
            {
                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
   
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    //filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.;
                    //filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    //filterContext.Result = new JsonResult
                    //{
                    //    Data = new {Error = "You Are Not Authorized!", Url = "/AccessDenied" },
                    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    //};                  

                    var result = new CoreJsonResult();
                    result.AddError("You are Not Authorized to View : " + controllerName + "/" + actionName);
                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    filterContext.Result = result;
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Account/AccessDenied");
                }               

                //var result = new CoreJsonResult();
                //result.AddError("You Are Not Authorized!");     

                //filterContext.Result = result;
            }
        }

        protected static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}
