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
	public class JabatanController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;
        private SqlMyPeopleDbContext _sqlMyPeopleDbContext = new SqlMyPeopleDbContext();
        private SqlHelper _query = new SqlHelper("SqlCoreConnection");

        public JabatanController(ApplicationDbContext context, ICurrentUser currentUser)
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
                var model = await _sqlMyPeopleDbContext.JabatanSet.FirstOrDefaultAsync(c => c.Id == id);

                return JsonSuccess(new { data = model });
            });

        }

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                int totalItems = _sqlMyPeopleDbContext.JabatanSet.Count();

                var models = await _sqlMyPeopleDbContext.JabatanSet
                .Where(x =>                        
                            string.IsNullOrEmpty(searchText) ||
                            (
                            x.Id.Contains(searchText) ||
                            x.Name.Contains(searchText)
                            )
                      
                )
                .OrderBy(sortBy + " " + sortDirection)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                IList<JabatanViewModel> list = Mapper.Map<IList<Jabatan>, IList<JabatanViewModel>>(models);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> All()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                var models = await _sqlMyPeopleDbContext.JabatanSet   
                           .ToListAsync();

                IList<JabatanViewModel> list = Mapper.Map<IList<Jabatan>, IList<JabatanViewModel>>(models);

                return JsonSuccess(new { data = list.ToArray() });
            });

        }

	}
}