using Core.Web.Controllers;
using Core.Web.Helpers;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using System.Web.Mvc;

namespace MyWeb.Portal.Controllers
{
    [Authorize]
    public class DashboardController : CoreControllerBase
    {
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;     
        private SqlHelper _query = new SqlHelper("SqlCoreConnection");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult mytask()
        {
            return View();
        }

        public ActionResult mycalender()
        {
            return View();
        }

        public ActionResult mynote()
        {
            return View();
        }

        public DashboardController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

    }
}