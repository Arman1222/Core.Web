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

namespace MyWeb.Controllers.Customers
{
    [MyAuthorize(Roles="Admin")]
	public class AreaController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;

        public AreaController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

		public ActionResult Index()
		{
			return View();
		}       

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText
            , int pageNumber = 1
            , int pageSize = 10
            , string sortBy = "AreaId"
            , string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {               
                int totalItems = _applicationDbContext.AreaSet.Count(
                    x =>
                            string.IsNullOrEmpty(searchText) ||
                            (
                            x.AreaCode.Contains(searchText) ||
                            x.AreaName.Contains(searchText)
                            )
                    );

                var models = await _applicationDbContext.AreaSet
                .Where(x =>                        
                            string.IsNullOrEmpty(searchText) ||
                            (
                            x.AreaCode.Contains(searchText) ||
                            x.AreaName.Contains(searchText)
                            )
                      
                )
                .OrderBy(sortBy + " " + sortDirection)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                IList<AreaViewModel> list = Mapper.Map<IList<Area>, IList<AreaViewModel>>(models);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> All()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                var models = await _applicationDbContext.AreaSet   
                           .ToListAsync();

                IList<AreaViewModel> list = Mapper.Map<IList<Area>, IList<AreaViewModel>>(models);

                return JsonSuccess(new { data = list.ToArray() });
            });

        }

	}
}