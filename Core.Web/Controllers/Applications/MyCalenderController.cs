using Core.Web.Controllers;
using Core.Web.Helpers;
using Core.Web.Infrastructure;
using Core.Web.Models.Applications;
using MyWeb.DataLayer;
using MyWeb.DataLayer.SqlMyPeople;
using MyWeb.Infrastructure.Applications;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.Controllers.Customers
{
    [MyAuthorize]
	public class MyCalenderController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;
        private SqlMyPeopleDbContext _sqlMyPeopleDbContext = new SqlMyPeopleDbContext();
        private SqlHelper _query = new SqlHelper("SqlCoreConnection");

        public MyCalenderController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> GetPageNote(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "posted_date", string sortDirection = "desc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    int totalItems = await ctx.MyNoteSet.Where(x => x.isDeleted == false && x.id_user == _currentUser.User.Id).CountAsync();

                    var models = await ctx.MyNoteSet.Where(x => x.isDeleted == false && x.id_user == _currentUser.User.Id)
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize).ToListAsync();

                    return JsonSuccess(new { totalItems = totalItems, data = models.ToArray() });
                }
            });
        }

        public async Task<JsonResult> AddNote(MyNote form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    if (!ModelState.IsValid)
                    {
                        return JsonValidationError();
                    }
                    form.posted_date = DateTime.Now;
                    form.id_user = _currentUser.User.Id;
                    form.id = Guid.NewGuid();
                    form.isDeleted = false;
                    ctx.MyNoteSet.Add(form);
                    ctx.SaveChanges();
                    return JsonSuccess(new { message = "success",  totalItems = form });
                }
            });
        }

        public async Task<JsonResult> UpdateNote(MyNote form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var target = ctx.MyNoteSet.Find(form.id);
                    if (target == null)
                    {
                        return JsonError("Data Tidak Ditemukan di database!");
                    }
                    target.Title = form.Title;
                    target.Description = form.Description;
                    target.note_class = form.note_class;
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { message = "success", totalItems = form });
                }
            });
        }

        public async Task<JsonResult> DeleteNote(MyNote form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var target = ctx.MyNoteSet.Find(form.id);
                    if (target == null)
                    {
                        return JsonError("Data Tidak Ditemukan di database!");
                    }
                    target.isDeleted = true;
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { message = "success", totalItems = form });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetPageTask(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    int totalItems = await ctx.MyTaskSet.Where(x => x.isDeleted == false && x.id_user == _currentUser.User.Id).CountAsync();

                    var models = await ctx.MyTaskSet.Where(x => x.isDeleted == false && x.id_user == _currentUser.User.Id
                            && (
                               string.IsNullOrEmpty(searchText) ||
                                (
                                x.Title.Contains(searchText) ||
                                x.Description.Contains(searchText)
                                )
                            )
                        )
                        .OrderBy(sortBy + " " + sortDirection)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize).ToListAsync();

                    return JsonSuccess(new { totalItems = totalItems, data = models.ToArray() });
                }
            });
        }

        public async Task<JsonResult> AddTask(MyTask form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    if (!ModelState.IsValid)
                    {
                        return JsonValidationError();
                    }
                    form.posted_by = _currentUser.User.Id;
                    form.posted_date = DateTime.Now;
                    form.progres = 0;
                    form.id_user = _currentUser.User.Id;
                    form.id = Guid.NewGuid();
                    ctx.MyTaskSet.Add(form);
                    ctx.SaveChanges();
                    return JsonSuccess(new { message = "success", totalItems = form });
                }
            });
        }

        public async Task<JsonResult> UpdateTask(MyTask form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var target = ctx.MyTaskSet.Find(form.id);
                    if (target == null)
                    {
                        return JsonError("Data Tidak Ditemukan di database!");
                    }
                    target.progres = form.progres;
                    target.note = form.note;
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { message = "success", totalItems = form });
                }
            });
        }

        public async Task<JsonResult> DeleteTask(MyTask form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var target = ctx.MyTaskSet.Find(form.id);
                    if (target == null)
                    {
                        return JsonError("Data Tidak Ditemukan di database!");
                    }
                    ctx.MyTaskSet.Remove(target);
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { message = "success", totalItems = form });
                }
            });
        }

	}
}