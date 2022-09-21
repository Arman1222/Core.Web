using Core.Web.Controllers;
using Core.Web.Helpers;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using MyWeb.Migrations.euser;
using MyWeb.ViewModels.area_master;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
//o20170928
namespace MyWeb.Controllers.euser
{
    public class master_area_Controller : CoreControllerBase
    {
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;
        private SqlHelper _query = new SqlHelper("UserConnection");
        private SqlUserDbContext ctx = new SqlUserDbContext();

        public master_area_Controller(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

        public ActionResult Index()
        {
            return View();
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "AreaCode", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlUserDbContext())
                {
                    int totalItems = await Task.FromResult<int>(ctx.sp_areaMaster_Set(searchText).Count());

                    var models = await Task.FromResult<IEnumerable<sp_areaMaster_Model>>(ctx.sp_areaMaster_Set(searchText).AsQueryable()
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize));

                    IList<sp_areaMaster_Model> list = models.ToList();

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }
        //o20170928
        public async Task<JsonResult> Add(sp_areaMaster_Model form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                string flag = "01";
                //int aktif = (int)form.ActiveBit;

                var Params = new object[] { 
                            new SqlParameter("@flag", flag)
                            //,new SqlParameter("@AreaId", form.AreaId)
                            ,new SqlParameter("@AreaCode", form.AreaCode)
                            ,new SqlParameter("@AreaName", form.AreaName)
                            //,new SqlParameter("@CreateDate", form.CreateDate)
                            //,new SqlParameter("@UpdateDate", form.UpdateDate)
                            //,new SqlParameter("@CreateBy", _currentUser.Username)
                            //,new SqlParameter("@UpdateBy", _currentUser.Username)
                            ,new SqlParameter("@By", _currentUser.Username)
                        };

                //await _query.ExecNonQueryAsync("[SP_AreaMaster] @flag,  @AreaCode, @AreaName, @CreateBy, @updateBy", Params);
                await _query.ExecNonQueryAsync("[SP_AreaMaster] @flag,  @AreaCode, @AreaName, @By", Params);

                return JsonSuccess("Insert Success!");
            });
        }
        
        public async Task<JsonResult> Update(sp_areaMaster_Model form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                string flag = "02";
                //int aktif = (int)form.ActiveBit;

                var Params = new object[] { 
                            new SqlParameter("@flag", flag)
                            //,new SqlParameter("@AreaId", form.AreaId)
                            ,new SqlParameter("@AreaCode", form.AreaCode)
                            ,new SqlParameter("@AreaName", form.AreaName)
                            //,new SqlParameter("@CreateDate", form.CreateDate)
                            //,new SqlParameter("@UpdateDate", form.UpdateDate)
                            //,new SqlParameter("@CreateBy", _currentUser.Username)
                            //,new SqlParameter("@UpdateBy", _currentUser.Username)
                            ,new SqlParameter("@By", _currentUser.Username)
                        };

                //await _query.ExecNonQueryAsync("[SP_AreaMaster] @flag,  @AreaCode, @AreaName, @CreateBy, @updateBy", Params);
                await _query.ExecNonQueryAsync("[SP_AreaMaster] @flag,  @AreaCode, @AreaName, @By", Params);

                return JsonSuccess("Update Success!");
            });
        }
                
        public async Task<string> CheckerCode(string search = "")
        {
            string flag = "13";
            string check = (string)await _query.ExecScalarProcAsync("[myweb_sp_get_table_grid]", "@flag", flag, "@search", search);
            return check;
        }
	}
    
}