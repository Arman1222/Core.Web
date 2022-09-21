using Core.Web.Controllers;
using Core.Web.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using MyWeb.DataLayer;
using MyWeb.Models;
using MyWeb.ViewModels.Menus;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Controllers.Applications
{
    [MyAuthorize(Roles = "Admin,ITSecurity")]
    public class MenuRoleController : CoreControllerBase
    {
        public MenuRoleController()
        {
        }

        public MenuRoleController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            MenuRoleViewModel menuRoleViewModel = new MenuRoleViewModel
            {
                Roles = RoleManager.Roles.ToList(),
                Menus = _applicationDbContext.Menus.ToList()
            };

            if (TempData["SelectedRoleId"] != null)
            {
                ViewBag.SelectedRoleId = TempData["SelectedRoleId"];
                TempData.Remove("SelectedRoleId");
            }

            return View(menuRoleViewModel);
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
        public async Task<ActionResult> Create([Bind(Include = "RoleId,MenuId,ApplicationName")] MenuRoleViewModel menuRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                //ApplicationRole applicationRole = new ApplicationRole { Name = applicationRoleViewModel.Name };

                //var roleResult = await RoleManager.CreateAsync(applicationRole);
                //if (!roleResult.Succeeded)
                //{
                //    ModelState.AddModelError("", roleResult.Errors.First());
                //    return View();
                //}

                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<JsonResult> Add([Bind(Include = "RoleId,MenuId,ApplicationName")] MenuRoleViewModel menuRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            ApplicationRole existingRole = await _applicationDbContext.Roles.FindAsync(menuRoleViewModel.RoleId);
            Navbar existingMenu = await _applicationDbContext.Menus.FindAsync(menuRoleViewModel.MenuId);

            if (existingRole == null)
            {
                return JsonError("Role tidak ada di database!");
            }
            if (existingMenu == null)
            {
                return JsonError("Menu tidak ada di database!");
            }

            Navbar existingParentMenu = null;
            if(existingMenu.ParentId != null){
                existingParentMenu = _applicationDbContext.Menus.FirstOrDefault(m => m.Id == existingMenu.ParentId && m.Roles.Count(r => r.Id == existingRole.Id) > 0);
                if (existingParentMenu == null)
                {
                    existingParentMenu = _applicationDbContext.Menus.FirstOrDefault(m => m.Id == existingMenu.ParentId);
                    return JsonError("Parent Menu : '" + existingParentMenu.Name + "' tidak ada dalam Role : '" + existingRole.Name + "'");
                }
            }
           
                try
                {
                    existingRole.Menus.Add(existingMenu);
                    await _applicationDbContext.SaveChangesAsync();                    
                }
                catch (Exception ex)
                {
                    return JsonError("Error Save Data : " + ex.Message);
                }           

            return await GetMenusByRole(menuRoleViewModel);      
        }

        public async Task<JsonResult> GetMenusByRole(MenuRoleViewModel menuRoleViewModel)
        {          
            //get menus for role
            NavbarController navbar = GetController<NavbarController>();
            string menuString = await navbar.GetMenusByRole(menuRoleViewModel.RoleId, menuRoleViewModel.ApplicationName);

            return JsonSuccess(menuString);
        }

        public async Task<ActionResult> DeleteMenuFromRole(int menuId , string roleId)
        {           
            ApplicationRole existingRole = await _applicationDbContext.Roles.FindAsync(roleId);
            Navbar existingMenu = await _applicationDbContext.Menus.FindAsync(menuId);

            //if (existingRole == null)
            //{
            //    return JsonError("Role tidak ada di database!");
            //}
            //if (existingMenu == null)
            //{
            //    return JsonError("Menu tidak ada di database!");
            //}

            try
            {
                existingRole.Menus.Remove(existingMenu);
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return JsonError("Error Save Data : " + ex.Message);
            }
            //http://stackoverflow.com/questions/19443444/redirect-to-action-with-parameters-always-null-in-mvc
            TempData["SelectedRoleId"] = roleId;
            return RedirectToAction("Index");  
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RoleManager.Dispose();
                _applicationDbContext.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
