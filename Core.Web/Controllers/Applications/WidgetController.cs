using Core.Web.Controllers;
using MyWeb.DataLayer;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
    public class WidgetController : CoreControllerBase
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }
    }
}
