using Core.Web.Controllers;
using Core.Web.Helpers;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using MyWeb.Migrations.euser;
using MyWeb.ViewModels.euser;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.Controllers.euser
{
    public class master_userController : CoreControllerBase
    {
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;
        private SqlHelper _query = new SqlHelper("UserConnection");
        private SqlUserDbContext ctx = new SqlUserDbContext();

        public master_userController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "Name", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlUserDbContext())
                {
                    int totalItems = await Task.FromResult<int>(ctx.sp_usermasterSet(searchText).Count());

                    var models = await Task.FromResult<IEnumerable<sp_usermasterModel>>(ctx.sp_usermasterSet(searchText).AsQueryable()
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize));

                    IList<sp_usermasterModel> list = models.ToList();

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }

        public async Task<JsonResult> Add(usermasterModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                int aktif = (int)form.ActiveBit;

                var Params = new object[] { 
                            new SqlParameter("@username", form.Name),
                            new SqlParameter("@userRealname", form.RealName),
                            new SqlParameter("@officeID", form.OfficeId),
                            new SqlParameter("@password", form.password),
                            new SqlParameter("@active", aktif),
                            new SqlParameter("@expired", form.ExpiredDate),
                            new SqlParameter("@maxPass", form.MaxPassWrong),
                            new SqlParameter("@userid", _currentUser.Username),
                            new SqlParameter("@NIK", form.NIK == null ? (object)DBNull.Value : form.NIK),
                        };

                await _query.ExecNonQueryAsync("[addUser] @username, @userRealname, @officeID, @password, @active, @expired, @maxPass, @userid, @NIK", Params);

                return JsonSuccess("Success!");
            });
        }

        public async Task<JsonResult> Update(usermasterModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                string flag = "01";

                int aktif = (int)form.ActiveBit;

                var Params = new object[] { 
                            new SqlParameter("@realname", form.RealName),
                            new SqlParameter("@officeid", form.OfficeId),
                            new SqlParameter("@active", aktif),
                            new SqlParameter("@expired", form.ExpiredDate),
                            new SqlParameter("@nik", form.NIK == null ? (object)DBNull.Value : form.NIK),
                            new SqlParameter("@maxPass", form.MaxPassWrong),
                            new SqlParameter("@username", form.Name),
                            new SqlParameter("@password", form.password == null? "" : form.password),
                            new SqlParameter("@flag", flag),
                            new SqlParameter("@userid", _currentUser.Username),
                        };

                await _query.ExecNonQueryAsync("[myweb_sp_update_usermaster] @realname, @officeid, @active, @expired, @nik, @maxPass, @username, @password, @flag, @userid", Params);

                return JsonSuccess("Update Success!");
            });
        }

        public async Task<JsonResult> UpdateWithPass(usermasterModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                string flag = "02";

                int aktif = (int)form.ActiveBit;

                var Params = new object[] { 
                            new SqlParameter("@realname", form.RealName),
                            new SqlParameter("@officeid", form.OfficeId),
                            new SqlParameter("@active", aktif),
                            new SqlParameter("@expired", form.ExpiredDate),
                            new SqlParameter("@nik", form.NIK == null ? (object)DBNull.Value : form.NIK),
                            new SqlParameter("@maxPass", form.MaxPassWrong),
                            new SqlParameter("@username", form.Name),
                            new SqlParameter("@password", form.password),
                            new SqlParameter("@flag", flag),
                            new SqlParameter("@userid", _currentUser.Username),
                        };

                await _query.ExecNonQueryAsync("[myweb_sp_update_usermaster] @realname, @officeid, @active, @expired, @nik, @maxPass, @username, @password, @flag, @userid", Params);

                return JsonSuccess("Update Success!");
            });
        }

        public async Task<JsonResult> Hapus(usermasterModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                string flag = "03";

                var Params = new object[] { 
                            new SqlParameter("@realname", string.Empty),
                            new SqlParameter("@officeid", string.Empty),
                            new SqlParameter("@active", DBNull.Value),
                            new SqlParameter("@expired", DBNull.Value),
                            new SqlParameter("@nik", string.Empty),
                            new SqlParameter("@maxPass", DBNull.Value),
                            new SqlParameter("@username", form.Name),
                            new SqlParameter("@password", string.Empty),
                            new SqlParameter("@flag", flag),
                            new SqlParameter("@userid", _currentUser.Username),
                        };

                await _query.ExecNonQueryAsync("[myweb_sp_update_usermaster] @realname, @officeid, @active, @expired, @nik, @maxPass, @username, @password, @flag, @userid", Params);

                return JsonSuccess("Delete Success!");
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetOffice(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "OfficeDesc", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlUserDbContext())
                {
                    int totalItems = await Task.FromResult<int>(ctx.sp_officemasterSet(searchText).Count());

                    var models = await Task.FromResult<IEnumerable<sp_officemasterModel>>(ctx.sp_officemasterSet(searchText).AsQueryable()
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize));

                    IList<sp_officemasterModel> list = models.ToList();

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetClassification(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "ClassificationDesc", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlUserDbContext())
                {
                    int totalItems = await Task.FromResult<int>(ctx.sp_userclassificationtypeSet(searchText).Count());

                    var models = await Task.FromResult<IEnumerable<sp_userclassificationtypeModel>>(ctx.sp_userclassificationtypeSet(searchText).AsQueryable()
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize));

                    IList<sp_userclassificationtypeModel> list = models.ToList();

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetModule(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "Module", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlUserDbContext())
                {
                    int totalItems = await Task.FromResult<int>(ctx.sp_modulemasterSet(searchText).Count());

                    var models = await Task.FromResult<IEnumerable<sp_modulemasterModel>>(ctx.sp_modulemasterSet(searchText).AsQueryable()
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize));

                    IList<sp_modulemasterModel> list = models.ToList();

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetUserModule(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "Module", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlUserDbContext())
                {
                    int totalItems = await Task.FromResult<int>(ctx.sp_usermodulSet(searchText).Count());

                    var models = await Task.FromResult<IEnumerable<sp_usermodulModel>>(ctx.sp_usermodulSet(searchText).AsQueryable()
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize));

                    IList<sp_usermodulModel> list = models.ToList();

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }

        public async Task<JsonResult> AddModule(moduleModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                var Params = new object[] { 
                            new SqlParameter("@username", form.Username),
                            new SqlParameter("@module", form.Module),
                            new SqlParameter("@clas", form.class_id),
                            new SqlParameter("@userid", _currentUser.Username),
                        };

                await _query.ExecNonQueryAsync("[assignUserModule] @username, @module, @clas, @userid", Params);

                return JsonSuccess("Success!");
            });
        }

        public async Task<JsonResult> HapusModule(moduleModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return JsonValidationError();

                var Params = new object[] { 
                            new SqlParameter("@username", form.Username),
                            new SqlParameter("@module", form.Module),
                            new SqlParameter("@userid", _currentUser.Username),
                        };

                await _query.ExecNonQueryAsync("[deleteUserModule] @username, @module, @userid", Params);

                return JsonSuccess("Delete Success!");
            });
        }

        public async Task<string> Checker(string search)
        {
            string flag = "06";
            string check = (string)await _query.ExecScalarProcAsync("[myweb_sp_get_table_grid]", "@flag", flag, "@search", search);
            return check;
        }
	}
}