using Core.Web.Controllers;
using Core.Web.Helpers;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using MyWeb.Migrations.euser;
using MyWeb.ViewModels.office_master;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
//o20170927
namespace MyWeb.Controllers.euser
{
    public class master_office_Controller : CoreControllerBase
    {
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;
        private SqlHelper _query = new SqlHelper("UserConnection");
        private SqlUserDbContext ctx = new SqlUserDbContext();

        public master_office_Controller(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

        public ActionResult Index()
        {
            return View();
        }
        //o20170927
        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlUserDbContext())
                {
                    int totalItems = await Task.FromResult<int>(ctx.sp_office_master_Set(searchText).Count());

                    var models = await Task.FromResult<IEnumerable<sp_office_master_Model>>(ctx.sp_office_master_Set(searchText).AsQueryable()
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize));

                    IList<sp_office_master_Model> list = models.ToList();

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }
        //o20170928
        public async Task<JsonResult> Add(sp_office_master_Model form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                string flag = "01";
                //int aktif = (int)form.ActiveBit;

                var Params = new object[] { 
                            new SqlParameter("@flag", flag)
                            ,new SqlParameter("@OfficeId", form.OfficeId)
                            ,new SqlParameter("@OfficeDesc", form.OfficeDesc)
                            ,new SqlParameter("@OfficeT24_KD_Cabang", form.OfficeT24_KD_Cabang)
                            ,new SqlParameter("@OfficeT24_Nama_Cabang", form.OfficeT24_Nama_Cabang)
                            //,new SqlParameter("@Id", Id)
                            //,new SqlParameter("@CreateDate", form.CreateDate)
                            //,new SqlParameter("@UpdateDate", form.UpdateDate)
                            ,new SqlParameter("@CreateBy", _currentUser.Username)
                            ,new SqlParameter("@UpdateBy", _currentUser.Username)
                            ,new SqlParameter("@AreaId", form.AreaId)
                            ,new SqlParameter("@AreaName", form.AreaName)
                        };

                //await _query.ExecNonQueryAsync("[SP_addOffice] @OfficeId, @OfficeDesc, @OfficeT24_KD_Cabang, @OfficeT24_Nama_Cabang, @CreateBy, @AreaId, @AreaName", Params);
                await _query.ExecNonQueryAsync("[SP_OfficeMaster] @flag, @OfficeId, @OfficeDesc, @OfficeT24_KD_Cabang, @OfficeT24_Nama_Cabang, @CreateBy, @UpdateBy, @AreaId, @AreaName", Params);

                return JsonSuccess("Insert Success!");
            });
        }
        //o20170928
        public async Task<JsonResult> Update(sp_office_master_Model form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                string flag = "02";
                //int aktif = (int)form.ActiveBit;

                var Params = new object[] { 
                            new SqlParameter("@flag", flag)
                            ,new SqlParameter("@OfficeId", form.OfficeId)
                            ,new SqlParameter("@OfficeDesc", form.OfficeDesc)
                            ,new SqlParameter("@OfficeT24_KD_Cabang", form.OfficeT24_KD_Cabang)
                            ,new SqlParameter("@OfficeT24_Nama_Cabang", form.OfficeT24_Nama_Cabang)
                            //,new SqlParameter("@Id", Id)
                            //,new SqlParameter("@CreateDate", form.CreateDate)
                            //,new SqlParameter("@UpdateDate", form.UpdateDate)
                            ,new SqlParameter("@CreateBy", _currentUser.Username)
                            ,new SqlParameter("@UpdateBy", _currentUser.Username)
                            ,new SqlParameter("@AreaId", form.AreaId)
                            ,new SqlParameter("@AreaName", form.AreaName)
                        };

                //await _query.ExecNonQueryAsync("[SP_updateOffice] @OfficeId, @OfficeDesc, @OfficeT24_KD_Cabang, @OfficeT24_Nama_Cabang, @UpdateBy, @AreaId, @AreaName", Params);
                await _query.ExecNonQueryAsync("[SP_OfficeMaster] @flag, @OfficeId, @OfficeDesc, @OfficeT24_KD_Cabang, @OfficeT24_Nama_Cabang, @CreateBy, @UpdateBy, @AreaId, @AreaName", Params);
                
                return JsonSuccess("Update Success!");
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetArea(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "AreaName", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlUserDbContext())
                {
                    int totalItems = await Task.FromResult<int>(ctx.sp_area_master_Set(searchText).Count());

                    var models = await Task.FromResult<IEnumerable<sp_area_master_Model>>(ctx.sp_area_master_Set(searchText).AsQueryable()
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize));

                    IList<sp_area_master_Model> list = models.ToList();

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }
        //o20170928
        public async Task<string> Checker(string search = "")
        {
            string flag = "10";
            string check = (string)await _query.ExecScalarProcAsync("[myweb_sp_get_table_grid]", "@flag", flag, "@search", search);
            return check;
        }
        //o20170929
        public async Task<string> CheckerT24(string search = "")
        {
            string flag = "11";
            string check = (string)await _query.ExecScalarProcAsync("[myweb_sp_get_table_grid]", "@flag", flag, "@search", search);
            return check;
        }
	}
    
}