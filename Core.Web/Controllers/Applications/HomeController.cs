using AutoMapper;
using Core.Web.Infrastructure;
using Core.Web.Models.Applications;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using MyWeb.Models;
using MyWeb.ViewModels.Menus;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
    public class HomeController : Controller
    {
        private ICurrentUser _currentUser;
        private ApplicationDbContext _applicationDbContext;
        public HomeController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

        [MyAuthorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated || Request.IsAuthenticated)
            {

            }
            using (var temenosCtx = new ApplicationDbContext())
            {
                string applicationName = WebConfigurationManager.AppSettings["ApplicationName"];
                Application app = _applicationDbContext.ApplicationSet.FirstOrDefault(a => a.ApplicationName == applicationName);
                var role = _currentUser.Roles;
                var _Model = temenosCtx.HomeMenuRoleSet.Where(x => role.Contains<string>(x.RoleId) && x.ApilcationId == app.ApplicationId).ToList();

                IList<HomeMenuRoleViewModel> list = Mapper.Map<IList<HomeMenuRole>, IList<HomeMenuRoleViewModel>>(_Model);
                return View(list);
            }
        }

        public ActionResult FlotCharts()
        {
            return View("FlotCharts");
        }

        public ActionResult MorrisCharts()
        {
            return View("MorrisCharts");
        }

        public ActionResult Tables()
        {
            return View("Tables");
        }

        public ActionResult Forms()
        {
            return View("Forms");
        }

        public ActionResult Panels()
        {
            return View("Panels");
        }

        public ActionResult Buttons()
        {
            return View("Buttons");
        }

        public ActionResult Notifications()
        {
            return View("Notifications");
        }

        public ActionResult Typography()
        {
            return View("Typography");
        }

        public ActionResult Icons()
        {
            return View("Icons");
        }

        public ActionResult Grid()
        {
            return View("Grid");
        }

        public ActionResult Blank()
        {
            return View("Blank");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

    }
}