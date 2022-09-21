using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Web.Controllers;
using Core.Web.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using MyWeb.DataLayer;
using MyWeb.Models;
using MyWeb.ViewModels.Applications;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
    [MyAuthorize(Roles = "Admin,ITSecurity")]
    public class ApplicationRolesController : CoreControllerBase
    {
        public ApplicationRolesController()
        {
        }

        public ApplicationRolesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                int totalItems = RoleManager.Roles.Count();

                var models = await RoleManager.Roles
                .Where(x => string.IsNullOrEmpty(searchText) ||
                        (
                        x.Name.Contains(searchText)                      
                        )
                )
                .OrderBy(sortBy + " " + sortDirection)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                IList<RoleViewModel> list = Mapper.Map<IList<ApplicationRole>, IList<RoleViewModel>>(models);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
        }

        public JsonResult All()
        {           
            var roleModels = RoleManager.Roles
                .OrderBy(x => x.Name)
                .Project().To<RoleViewModel>();

            return JsonSuccess(roleModels.ToArray());
        }
        [AllowAnonymous]
        public JsonResult RoleGetAllRole()
        {
            var roleModels = RoleManager.Roles
                .OrderBy(x => x.Name)
                .Project().To<RoleViewModel>();

            return JsonSuccess( new { data = roleModels.ToArray() });
        }
        [AllowAnonymous]
        public JsonResult RoleGetAllNavbar(string roleId, Nullable<int> applicationId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var Models = ctx.Sp_get_navbar_by_role_app(roleId, applicationId).ToList();
                var TreeModel = Models.GenerateTree(c => c.MenuId, c => c.ParentId).ToList();
                return JsonSuccess(new { data = TreeModel.ToArray() });
            }
        }
        [AllowAnonymous]
        public JsonResult RoleGetAllApplication(string roleId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var Models = ctx.Sp_get_application_by_role(roleId).ToList();
                return JsonSuccess(new { data = Models.ToArray() });
            }
        }
        [AllowAnonymous]
        public async Task<JsonResult> RoleAdd(ApplicationRoleViewModel model)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var c = ctx.Roles.Where(x => x.Name == model.Name).FirstOrDefault();
                    if (c != null)
                    {
                        return JsonError("Menu dengan nama " + model.Name + " sudah ada dalam database");
                    }
                    ApplicationRole appR = new ApplicationRole { Name = model.Name };
                    ctx.Roles.Add(appR);
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { data = model });
                }

            });
        }
        [AllowAnonymous]
        public async Task<JsonResult> RoleUpdate(ApplicationRoleViewModel model)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var c = ctx.Roles.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (c == null)
                    {
                        return JsonError("Menu tidak ada dalam database");
                    }
                    var b = ctx.Roles.Where(x => x.Name == model.Name && x.Id != model.Id).FirstOrDefault();
                    if (b != null)
                    {
                        return JsonError("Menu dengan nama " + model.Name + " sudah ada dalam database");
                    }
                    c.Name = model.Name;
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { data = model });
                }
            });
        }
        [AllowAnonymous]
        public async Task<JsonResult> RoleDelete(ApplicationRoleViewModel model)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var c = ctx.Roles.Where(x => x.Id == model.Id).FirstOrDefault();

                    if (c == null)
                    {
                        return JsonError("Menu tidak ada dalam database");
                    }
                    ctx.Roles.Remove(c);
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { menu = model });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> LinkRoleNavbar(string roleId, Nullable<System.Guid> navbarId)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.sp_insert_menurole(roleId, navbarId);
                    return JsonSuccess(new { data = "succes" });
                }
            });
        }
        [AllowAnonymous]
        public async Task<JsonResult> UnlinkRoleNavbar(string roleId, Nullable<System.Guid> navbarId)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.sp_delete_menurole(roleId, navbarId);
                    return JsonSuccess(new { data = "succes" });
                }
            });
        }
        


        public async Task<ActionResult> Index()
        {
            return View();
        }


        // GET: ApplicationRoles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // GET: ApplicationRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole applicationRole = new ApplicationRole { Name = applicationRoleViewModel.Name };

                var roleResult = await RoleManager.CreateAsync(applicationRole);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", roleResult.Errors.First());
                    return View();
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: ApplicationRoles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            ApplicationRoleViewModel applicationRoleViewModel = new ApplicationRoleViewModel
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name
            };
            return View(applicationRoleViewModel);
        }

        // POST: ApplicationRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole applicationRole = await RoleManager.FindByIdAsync(applicationRoleViewModel.Id);
                string originalName = applicationRole.Name;

                if (originalName == "Admin" && applicationRoleViewModel.Name != "Admin")
                {
                    ModelState.AddModelError("", "You cannot change the name of the Admin role.");
                    return View(applicationRoleViewModel);
                }

                if (originalName != "Admin" && applicationRoleViewModel.Name == "Admin")
                {
                    ModelState.AddModelError("", "You cannot change the name of a role to Admin.");
                    return View(applicationRoleViewModel);
                }

                applicationRole.Name = applicationRoleViewModel.Name;
                await RoleManager.UpdateAsync(applicationRole);

                return RedirectToAction("Index");
            }
            return View(applicationRoleViewModel);
        }

        // GET: ApplicationRoles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);

            if (applicationRole.Name == "Admin")
            {
                ModelState.AddModelError("", "You cannot delete the Admin role.");
                return View(applicationRole);
            }

            await RoleManager.DeleteAsync(applicationRole);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RoleManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
