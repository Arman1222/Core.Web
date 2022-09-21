using AutoMapper;
using Core.Web.Controllers;
using Core.Web.Helpers;
using Core.Web.Infrastructure;
using MyWeb.DataLayer;
using MyWeb.DataLayer.SqlMyPeople;
using MyWeb.Infrastructure.Applications;
using MyWeb.Models.Applications;
using MyWeb.ViewModels.Applications;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.Controllers.Customers
{
    [MyAuthorize]
	public class EmployeeController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;
        private SqlMyPeopleDbContext _sqlMyPeopleDbContext = new SqlMyPeopleDbContext();
        private SqlHelper _query = new SqlHelper("SqlCoreConnection");

        public EmployeeController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

		public ActionResult Index()
		{
			return View();
		}

        [AllowAnonymous]
        public async Task<JsonResult> GetById(string id)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                var model = await _sqlMyPeopleDbContext.Employee.FirstOrDefaultAsync(c => c.NIK == id);

                return JsonSuccess(new { data = model });
            });

        }

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "nik", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                
                var models = _sqlMyPeopleDbContext.Employee
                .Where(x =>                        
                            string.IsNullOrEmpty(searchText) ||
                            (
                            x.NIK.Contains(searchText) ||
                            x.Nama.Contains(searchText) ||
                            x.Cabang.Contains(searchText) ||
                            x.Department.Contains(searchText) ||
                            x.Director.Contains(searchText) ||
                            x.Division.Contains(searchText) ||
                            x.Group.Contains(searchText) ||
                            x.Jabatan.Contains(searchText)                          
                            )
                      
                );

                int totalItems = await models.CountAsync();

                var listModels = await models
                .OrderBy(sortBy + " " + sortDirection)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                IList<EmployeeViewModel> list = Mapper.Map<IList<Employee>, IList<EmployeeViewModel>>(listModels);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> All()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                var models = await _sqlMyPeopleDbContext.Employee   
                           .ToListAsync();

                IList<EmployeeViewModel> list = Mapper.Map<IList<Employee>, IList<EmployeeViewModel>>(models);

                return JsonSuccess(new { data = list.ToArray() });
            });

        }

	}
}