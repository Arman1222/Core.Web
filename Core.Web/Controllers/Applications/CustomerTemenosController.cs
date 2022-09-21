using AutoMapper;
using Core.Web.Controllers;
using Core.Web.Helpers;
using MyWeb.DataLayer;
using MyWeb.DataLayer.SqlTemenos;
using MyWeb.Infrastructure.Applications;
using MyWeb.Models.Applications;
using MyWeb.ViewModels.Applications;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.Controllers.Customers
{    
	public class CustomerTemenosController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;       
        private SqlHelper _query = new SqlHelper("SqlTemenosConnection");

        public CustomerTemenosController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

		public ActionResult Index()
		{
			return View();
		}
      
        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "CifNo", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {                
                using(var temenosCtx = new SqlTemenosDbContext()){

                    //temenosCtx.Database.Log = Console.Write;
                    temenosCtx.Database.Log = message => Trace.Write(message);

                    IList<CustomerTemenos> listModel = new List<CustomerTemenos>();
                    int totalItems = 0;
                    var models = temenosCtx.CustomerTemenosSet
                    .Where(x =>                        
                                string.IsNullOrEmpty(searchText) ||
                                (
                                x.CifNo.Contains(searchText) ||
                                x.Nama.Contains(searchText) ||
                                x.Alamat.Contains(searchText)
                                )                      
                    );

                    totalItems = models.Count();

                    listModel = models
                    .OrderBy(sortBy + " " + sortDirection)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                    IList<CustomerTemenosViewModel> list = Mapper.Map<IList<CustomerTemenos>, IList<CustomerTemenosViewModel>>(listModel);
    
                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });

                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> All()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var temenosCtx = new SqlTemenosDbContext())
                {
                    var models = await temenosCtx.CustomerTemenosSet
                               .ToListAsync();

                    IList<CustomerTemenosViewModel> list = Mapper.Map<IList<CustomerTemenos>, IList<CustomerTemenosViewModel>>(models);

                    return JsonSuccess(new { data = list.ToArray() });
                }
            });

        }

	}
}