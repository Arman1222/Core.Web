using AutoMapper;
using Core.Web.Controllers;
using Core.Web.Infrastructure;
using MyWeb.DataLayer;
using MyWeb.Models;
using MyWeb.ViewModels.Menus;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.Controllers.Applications
{
    [MyAuthorize(Roles = "Admin,ITSecurity")]
    public class HomeMenuRoleController : CoreControllerBase
    {
        public HomeMenuRoleController()
        {
        }

        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var models = ctx.HomeMenuRoleSet
                    .Where(x => x.IsDelete == false)
                    .ToList();

                    int totalItems = models.AsQueryable().Count(
                            x =>
                                string.IsNullOrEmpty(searchText) ||
                                (
                                    x.FolderView.Contains(searchText) ||
                                    x.NameCshtml.Contains(searchText) ||
                                    x.Role.Name.Contains(searchText)
                                )
                            );

                    IList<HomeMenuRoleViewModel> list = Mapper.Map<IList<HomeMenuRole>, IList<HomeMenuRoleViewModel>>(models.AsQueryable().Where(
                            x =>
                                string.IsNullOrEmpty(searchText) ||
                                (
                                    x.FolderView.Contains(searchText) ||
                                    x.NameCshtml.Contains(searchText) ||
                                    x.Role.Name.Contains(searchText)
                                )
                            )
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList());

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> All()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var models = ctx.HomeMenuRoleSet
                               .Where(x => x.IsDelete == false)
                               .OrderByDescending(x => x.id)
                               .ToList();

                    IList<HomeMenuRoleViewModel> list = Mapper.Map<IList<HomeMenuRole>, IList<HomeMenuRoleViewModel>>(models);

                    return JsonSuccess(new { data = list.ToArray() });
                }
            });

        }

        public async Task<JsonResult> Add(HomeMenuRoleViewModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return JsonValidationError();
                }

                HomeMenuRole model = Mapper.Map<HomeMenuRole>(form);

                using (var ctx = new ApplicationDbContext())
                {
                    var existingModel = ctx.HomeMenuRoleSet.FirstOrDefault(c => c.id == form.id);
                    if (existingModel != null)
                    {
                        return JsonError("Data Sudah Ada di database!");
                    }

                    ctx.HomeMenuRoleSet.Add(model);
                    await ctx.SaveChangesAsync();

                    return JsonSuccess("");
                }
            });
        }

        public async Task<JsonResult> Update(HomeMenuRoleViewModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var existingModel = ctx.HomeMenuRoleSet.Find(form.id);
                    if (existingModel == null)
                    {
                        return JsonError("Data Tidak Ditemukan di database!");
                    }

                    Mapper.Map(form, existingModel);
                    await ctx.SaveChangesAsync();
                    //HomeMenuRoleViewModel updatedModel = ctx.HomeMenuRoleSet.Project().To<HomeMenuRoleViewModel>().Single(x => x.Id == form.Id);

                    return JsonSuccess("Succes");
                }
            });
        }

        public async Task<JsonResult> Delete(HomeMenuRoleViewModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var target = ctx.HomeMenuRoleSet.Find(form.id);
                    if (target == null)
                    {
                        return JsonError("Data Tidak Ditemukan di database!");
                    }
                    ctx.HomeMenuRoleSet.Remove(target);

                    await ctx.SaveChangesAsync();

                    //HomeMenuRoleViewModel updatedModel = ctx.HomeMenuRoleSet.Project().To<HomeMenuRoleViewModel>().Single(x => x.Id == form.Id);

                    return JsonSuccess("Delete Success!");
                }
            });
        }

    }
}
