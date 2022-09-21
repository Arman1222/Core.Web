using AutoMapper;
using Core.Web.Controllers;
using Core.Web.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using MyWeb.DataLayer;
using MyWeb.DataLayer.SqlMyPeople;
using MyWeb.Infrastructure.Applications;
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
    public class ApplicationUsersController : CoreControllerBase
    {
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;    
        public ApplicationUsersController()
        {
        }

        public ApplicationUsersController(ApplicationDbContext context, ICurrentUser currentUser, ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
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
            private set
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

        [AllowAnonymous]
        public async Task<JsonResult> All()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                var models = await _applicationDbContext.Users
                           .ToListAsync();

                IList<ApplicationUserViewModel> list = Mapper.Map<IList<ApplicationUser>, IList<ApplicationUserViewModel>>(models);

                return JsonSuccess(new { data = list.ToArray() });
            });
        }

        [AllowAnonymous]
        public JsonResult UserRoleGetAll(string userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var Models = ctx.Sp_get_role_by_user(userId).ToList();
                return JsonSuccess(new { data = Models.ToArray() });
            }
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> UserRoleAdd(string userId, string roleId)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.sp_insert_userrole(userId,roleId);
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { data = "succes" });
                }

            });
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> UserRoleDelete(string userId, string roleId)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.sp_delete_userrole(userId, roleId);
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { data = "succes" });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> getMypeopleDashBoardData()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlMyPeopleDbContext())
                {
                    var data = ctx.MyPeople_GetDashBoardData(_currentUser.Employee.NIK).ToList();
                    return JsonSuccess(new { data = data });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetEmployee()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {               
                return JsonSuccess(_currentUser.Employee);

            });
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> GetDepartment()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                return JsonSuccess(_currentUser.Department);

            });
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> GetDivision()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                return JsonSuccess(_currentUser.Division);

            });
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> GetJabatan()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                return JsonSuccess(_currentUser.Jabatan);

            });
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> GetArea()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                return JsonSuccess(Mapper.Map<AreaViewModel>(_currentUser.Area));

            });
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> GetRoleNames()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                return JsonSuccess(_currentUser.RoleNames);
            });
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> GetBranch()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                return JsonSuccess(Mapper.Map<BranchViewModel>(_currentUser.Branch));

            });
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText = "", int pageNumber = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "desc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {               
                    var models = UserManager.Users
                    .Where(c =>
                                (
                                   c.UserName.Contains(searchText) ||
                                   c.NIK.Contains(searchText) ||
                                   c.FullName.Contains(searchText) ||
                                   c.FirstName.Contains(searchText) ||
                                   c.LastName.Contains(searchText)
                                )
                    );

                    int totalItems = await models.CountAsync();

                    var listModel = await models
                    .OrderBy(sortBy + " " + sortDirection)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                    IList<ApplicationUserViewModel> list = Mapper.Map<IList<ApplicationUser>, IList<ApplicationUserViewModel>>(listModel);

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                
            });
        }

        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public async Task<ActionResult> Index()
        {
            return View();//View(await UserManager.Users.ToListAsync());
        }

        //// GET: ApplicationUsers/Details/5
        //public async Task<ActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ApplicationUser applicationUser = await db.ApplicationUsers.FindAsync(id);
        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(applicationUser);
        //}

        //// GET: ApplicationUsers/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ApplicationUsers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,Address,City,State,ZipCode,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ApplicationUsers.Add(applicationUser);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(applicationUser);
        //}

        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(applicationUser.Id);
            applicationUser.RolesList = RoleManager.Roles.OrderBy(x=>x.Name).ToList().Select(r => new SelectListItem
                                                                               {
                                                                                   Selected = userRoles.Contains(r.Name),
                                                                                   Text = r.Name,
                                                                                   Value = r.Name
                                                                               });

            ViewBag.BranchIdSelectList = PopulateBranchIdSelectList();

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BranchId,Nik")] ApplicationUser applicationUser, params string[] rolesSelectedOnView)
        {
            if (ModelState.IsValid)
            {
                //update branch
                if (applicationUser.BranchId != null || applicationUser.NIK != null)
                {
                    var user = await UserManager.FindByIdAsync(applicationUser.Id);
                    user.BranchId = applicationUser.BranchId;
                    user.NIK = applicationUser.NIK;
                    await UserManager.UpdateAsync(user);
                }
                
                // If the user is currently stored having the Admin role,
                var rolesCurrentlyPersistedForUser = await UserManager.GetRolesAsync(applicationUser.Id);
                bool isThisUserAnAdmin = rolesCurrentlyPersistedForUser.Contains("Admin");

                // and the user did not have the Admin role checked,
                rolesSelectedOnView = rolesSelectedOnView ?? new string[] { };
                bool isThisUserAdminDeselected = !rolesSelectedOnView.Contains("Admin");

                // and the current stored count of users with the Admin role == 1,
                var role = await RoleManager.FindByNameAsync("Admin");
                bool isOnlyOneUserAnAdmin = role.Users.Count == 1;

                // (populate the roles list in case we have to return to the Edit view)
                applicationUser = await UserManager.FindByIdAsync(applicationUser.Id);
                applicationUser.RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = rolesCurrentlyPersistedForUser.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });

                // then prevent the removal of the Admin role.
                if (isThisUserAnAdmin && isThisUserAdminDeselected && isOnlyOneUserAnAdmin)
                {
                    ModelState.AddModelError("", "At least one user must retain the Admin role; you are attempting to delete the Admin role from the last user who has been assigned to it.");
                    ViewBag.BranchIdSelectList = PopulateBranchIdSelectList();
                    return View(applicationUser);
                }
               
                var result = await UserManager.AddToRolesAsync(
                    applicationUser.Id, 
                    rolesSelectedOnView.Except(rolesCurrentlyPersistedForUser).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    ViewBag.BranchIdSelectList = PopulateBranchIdSelectList();
                    return View(applicationUser);
                }

                result = await UserManager.RemoveFromRolesAsync(
                    applicationUser.Id, 
                    rolesCurrentlyPersistedForUser.Except(rolesSelectedOnView).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    ViewBag.BranchIdSelectList = PopulateBranchIdSelectList();
                    return View(applicationUser);
                }

                return RedirectToAction("Index");
            }

            ViewBag.BranchIdSelectList = PopulateBranchIdSelectList();
            ModelState.AddModelError("", "Something failed.");
            return View(applicationUser);
        }

        private SelectList PopulateBranchIdSelectList()
        {
            return new SelectList(
                    _applicationDbContext
                    .BranchSet
                    .ToList(), "BranchId", "BranchName");
        }

        public async Task<ActionResult> LockAccount([Bind(Include = "Id")] string id)
        {
            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEndDateAsync(id, DateTime.UtcNow.AddYears(100));
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> UnlockAccount([Bind(Include = "Id")] string id)
        {
            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEndDateAsync(id, DateTime.UtcNow.AddYears(-1));
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
