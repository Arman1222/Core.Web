using AutoMapper;
using Core.Web.Controllers;
using Core.Web.Infrastructure;
using Core.Web.Models.Applications;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using MyWeb.ViewModels.Applications;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.Controllers.Applications
{
    [MyAuthorize(Roles = "Admin,ITSecurity")]
	public class ApplicationController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;      
        public ApplicationController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

		public ActionResult Index()
		{
			return View();
		}

        [AllowAnonymous]
        public async Task<JsonResult> GetAll(string sortBy = "ApplicationId", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {                   

                    var models = ctx.ApplicationSet;

                    int totalItems = await models.CountAsync();

                    var listModel = await models
                    .OrderBy(sortBy + " " + sortDirection)                    
                    .ToListAsync();

                    IList<ApplicationViewModel> list = Mapper.Map<IList<Application>, IList<ApplicationViewModel>>(listModel);

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "ApplicationName", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {

                    var models = ctx.ApplicationSet
                    .Where(x => string.IsNullOrEmpty(searchText) ||
                            (
                            x.ApplicationName.Contains(searchText) ||
                            x.ApplicationDescription.Contains(searchText) ||
                            x.ApplicationLink.Contains(searchText) ||
                            x.ApplicationIcon.Contains(searchText) 
                            )
                    );

                    int totalItems = await models.CountAsync();

                    var listModel = await models
                    .OrderBy(sortBy + " " + sortDirection)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                    IList<ApplicationViewModel> list = Mapper.Map<IList<Application>, IList<ApplicationViewModel>>(listModel);

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }

        public JsonResult All()
		{
            return ExecuteFaultHandledOperation(() =>
            {
                int totalItems = _applicationDbContext.ApplicationSet.Count();

                var models = _applicationDbContext.ApplicationSet
               .OrderByDescending(x => x.CreateDate)               
               .ToList();

                IList<ApplicationViewModel> list = Mapper.Map<IList<Application>, IList<ApplicationViewModel>>(models);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
		}

        public JsonResult Add(ApplicationViewModel form)
		{
            return ExecuteFaultHandledOperation(() =>
            {
                if (!ModelState.IsValid)
                {
                    return JsonValidationError();
                }

                Application model = Mapper.Map<Application>(form);
                _applicationDbContext.ApplicationSet.Add(model);
                _applicationDbContext.SaveChanges();

                return JsonSuccess(Mapper.Map<ApplicationViewModel>(model));
            });
		}

        public JsonResult Update(ApplicationViewModel form)
		{
            return ExecuteFaultHandledOperation(() =>
            {
                var target = _applicationDbContext.ApplicationSet.Find(form.ApplicationId);
                if (target == null)
                {
                    return JsonError("Application Tidak Ditemukan di database!");
                }
                     
                Mapper.Map(form, target);
                target.SetUpdateByCurrentUser();
                _applicationDbContext.SaveChanges();

                ApplicationViewModel updatedModel = Mapper.Map<ApplicationViewModel>(target);

                return JsonSuccess(updatedModel);
            });
		}
	}
}