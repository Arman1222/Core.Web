using AutoMapper;
using Core.Web.Controllers;
using Core.Web.Helpers;
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
	public class BranchController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;       
        private SqlHelper _query = new SqlHelper("SqlCoreConnection");

        public BranchController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

		public ActionResult Index()
		{
			return View();
		}

        [AllowAnonymous]
        public async Task<JsonResult> GetById(int branchId)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                var model = await _applicationDbContext.BranchSet.FirstOrDefaultAsync(c=>c.BranchId == branchId);                                

                return JsonSuccess(new { data = model });
            });

        }

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "BranchId", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {            
                var models = _applicationDbContext.BranchSet
                .Where(x =>                        
                            string.IsNullOrEmpty(searchText) ||
                            (
                            x.BranchCode.Contains(searchText) ||
                            x.BranchName.Contains(searchText) ||
                            x.BranchCodeT24.Contains(searchText) ||
                            x.BranchNameT24.Contains(searchText) ||
                            x.AreaName.Contains(searchText)
                            )
                      
                );

                int totalItems = await models.CountAsync();

                var listModel = await models
                .OrderBy(sortBy + " " + sortDirection)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                IList<BranchViewModel> list = Mapper.Map<IList<Branch>, IList<BranchViewModel>>(listModel);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> All()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                var models = await _applicationDbContext.BranchSet   
                           .ToListAsync();

                IList<BranchViewModel> list = Mapper.Map<IList<Branch>, IList<BranchViewModel>>(models);

                return JsonSuccess(new { data = list.ToArray() });
            });

        }

	}
}