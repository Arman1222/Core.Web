using Core.Web.Controllers;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
	public class TemplateController : CoreControllerBase
	{
		public PartialViewResult Render(string feature, string name)
		{
			return PartialView(string.Format("~/js/app/{0}/templates/{1}", feature, name));
		}
	}
}