using AutoMapper;
using Core.Web.Controllers;
using Core.Web.Infrastructure;
using Core.Web.Models.Applications;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using MyWeb.ViewModels.Applications;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyWeb.Controllers.Applications
{
    [MyAuthorize(Roles = "Admin")]
	public class CompanyController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;

        public CompanyController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

		public ActionResult Index()
		{
			return View();
		}

        public JsonResult All(int pageNumber = 1, int pageSize = 5)
		{
            return ExecuteFaultHandledOperation(() =>
            {
                int totalItems = _applicationDbContext.CompanySet.Count();

                var models = _applicationDbContext.CompanySet
                .OrderByDescending(x => x.CreateDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                IList<CompanyViewModel> list = Mapper.Map<IList<Company>, IList<CompanyViewModel>>(models);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
                
		}

        public JsonResult Add(CompanyViewModel form)
		{
            return ExecuteFaultHandledOperation(() =>
            {
                if (!ModelState.IsValid)
                {
                    return JsonValidationError();
                }

                Company model = Mapper.Map<Company>(form);
                _applicationDbContext.CompanySet.Add(model);
                _applicationDbContext.SaveChanges();
                return JsonSuccess(Mapper.Map<CompanyViewModel>(model));
            });
		}

        public JsonResult Update(CompanyViewModel form)
		{
            return ExecuteFaultHandledOperation(() =>
            {
                var target = _applicationDbContext.CompanySet.Find(form.CompanyId);
                if (target == null)
                {
                    return JsonError("Company Id Tidak Ditemukan di database!");
                }
             
                Mapper.Map(form, target);
                _applicationDbContext.SaveChanges();

                CompanyViewModel updatedModel = Mapper.Map<CompanyViewModel>(target);

                return JsonSuccess(updatedModel);
            });
		}

	}
}