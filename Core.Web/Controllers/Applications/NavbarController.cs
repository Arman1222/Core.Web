using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Web.Controllers;
using Core.Web.Infrastructure;
using Core.Web.Models.Applications;
using Core.Web.Utilities;
using Microsoft.AspNet.Identity.Owin;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using MyWeb.Models;
using MyWeb.ViewModels.Applications;
using MyWeb.ViewModels.Menus;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
    internal static class GenericHelpers
    {
        /// <summary>
        /// Generates tree of items from item list
        /// </summary>
        /// 
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <typeparam name="K">Type of parent_id</typeparam>
        /// 
        /// <param name="collection">Collection of items</param>
        /// <param name="id_selector">Function extracting item's id</param>
        /// <param name="parent_id_selector">Function extracting item's parent_id</param>
        /// <param name="root_id">Root element id</param>
        /// 
        /// <returns>Tree of items</returns>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> id_selector,
            Func<T, K> parent_id_selector,
            K root_id = default(K))
        {
            foreach (var c in collection.Where(c => parent_id_selector(c).Equals(root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }
    }

    [MyAuthorize(Roles = "Admin,ITSecurity")]
    public class NavbarController : CoreControllerBase
    {
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

        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;       

        public NavbarController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

        // GET: Navbar
        //http://stackoverflow.com/questions/23107030/asp-net-login-redirect-loop-when-user-not-in-role
        //error infinite loop return url jika tidak allowanonymous karena filter authorize attribute
        //[AllowAnonymous]
        //public ActionResult Index()
        //{
            //var data = new Data();            
            //return PartialView("_Navbar", data.navbarItems().ToList());           
        //}

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                int totalItems = _applicationDbContext.Menus.Count();
               
                var models = await _applicationDbContext.Menus
                .Where(x => string.IsNullOrEmpty(searchText) ||
                        (
                        x.Name.Contains(searchText) ||
                        x.Text.Contains(searchText)
                        )
                )
                .OrderBy(sortBy + " " + sortDirection)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                IList<MenuViewModel> list = Mapper.Map<IList<Navbar>, IList<MenuViewModel>>(models);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
        }

        public JsonResult All()
        {
            var menuModels = _applicationDbContext.Menus
                .OrderBy(x => x.Name)
                .Project().To<MenuViewModel>();

            return JsonSuccess(menuModels.ToArray());
        }

        [AllowAnonymous]
        public async Task<JsonResult> Json_Gettreemenu()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                string[] roles = _currentUser.Roles;

                ViewBag.UserName = _currentUser.Username;
                ViewBag.FullName = _currentUser.User.FullName;

                ApplicationRole adminRole = RoleManager.Roles.FirstOrDefault(r => r.Name == "Admin");
                IList<Navbar> listOfNodes = null;
                if (adminRole != null && roles.Contains(adminRole.Id)) //if admin role exists in this user show all menu
                {
                    listOfNodes = GetListOfNodes(false);
                }
                else
                {
                    listOfNodes = GetListOfNodesByRoleAndApplication(roles);
                }
                var topLevelMenus = listOfNodes.GenerateTree(c => c.Id, c => c.ParentId).ToList();
                return JsonSuccess(new { data = topLevelMenus.ToArray() });
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> Json_Gettreemenubyapp(string application = "")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                IList<Navbar> listOfNodes = GetListOfNodesByApplication(application);
                var topLevelMenus = listOfNodes.GenerateTree(c => c.Id, c => c.ParentId).ToList();
                return JsonSuccess(new { data = topLevelMenus.ToArray() });
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> Json_Getmenu()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                string[] roles = _currentUser.Roles;

                ViewBag.UserName = _currentUser.Username;
                ViewBag.FullName = _currentUser.User.FullName;

                ApplicationRole adminRole = RoleManager.Roles.FirstOrDefault(r => r.Name == "Admin");
                IList<Navbar> listOfNodes = null;
                if (adminRole != null && roles.Contains(adminRole.Id)) //if admin role exists in this user show all menu
                {
                    listOfNodes = GetListOfNodes(false);
                }
                else
                {
                    listOfNodes = GetListOfNodesByRoleAndApplication(roles);
                }
                return JsonSuccess(new { data = listOfNodes.ToArray() });
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> Json_addmenu(Navbar menu)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var c = ctx.Menus.Where(x => x.Name == menu.Name && x.IsDelete == false).FirstOrDefault();
                    if (c != null)
                    {
                        return JsonError("Menu dengan nama " + menu.Name + " sudah ada dalam database");
                    }
                    menu.Id = Guid.NewGuid();
                    menu.CreateBy = _currentUser.User.Id;
                    menu.IsDelete = false;
                    menu.SortId = GetMaxsort(menu.ParentId, menu.ApplicationId) + 1;
                    menu.CreateDate = DateTime.Now;
                    ctx.Menus.Add(menu);
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { menu = menu });
                }
                
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> Json_updatemenu(Navbar menu)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var c = ctx.Menus.Where(x => x.Id == menu.Id && x.IsDelete == false).FirstOrDefault();
                    if (c ==null)
                    {
                        return JsonError("Menu tidak ada dalam database");
                    }
                    var b = ctx.Menus.Where(x => x.Name == menu.Name && x.Id != menu.Id && x.IsDelete == false).FirstOrDefault();
                    if (b != null)
                    {
                        return JsonError("Menu dengan nama " + menu.Name + " sudah ada dalam database");
                    }
                    c.Name = menu.Name;
                    c.Text = menu.Text;
                    c.Controller = menu.Controller;
                    c.Action = menu.Action;
                    c.ImageClass = menu.ImageClass;
                    c.SortId = menu.SortId;
                    c.faicon = menu.faicon;
                    c.description = menu.description;
                    c.ApplicationId = menu.ApplicationId;
                    c.ParentId = menu.ParentId;
                    c.UpdateBy = _currentUser.User.Id;
                    c.UpdateDate = DateTime.Now;
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { menu = menu });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> Json_deletemenu(Navbar menu)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var c = ctx.Menus.Where(x => x.Id == menu.Id && x.IsDelete == false).FirstOrDefault();
                    
                    if (c == null)
                    {
                        return JsonError("Menu tidak ada dalam database");
                    }
                    var child = ctx.Menus.Where(x => x.ParentId == c.Id && c.ApplicationId == x.ApplicationId && x.IsDelete == false);
                    if (child != null)
                    {
                        ctx.Menus.RemoveRange(child);
                    }
                    ctx.Menus.Remove(c);
                    
                    await ctx.SaveChangesAsync();
                    var brother = ctx.Menus.Where(x => (x.ParentId == menu.ParentId) && menu.ApplicationId == x.ApplicationId && x.IsDelete == false).OrderBy(x => x.SortId);
                    if (brother != null)
                    {
                        int sorttemp = 1;
                        foreach (var item in brother)
                        {
                            item.SortId = sorttemp++;
                        }
                    }
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { menu = menu });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> Json_moveupmenu(Navbar menu)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var c = ctx.Menus.Where(x => x.Id == menu.Id && x.IsDelete == false).FirstOrDefault();
                    if (c == null)
                    {
                        return JsonError("Menu tidak ada dalam database");
                    }
                    if (c.SortId == null || c.SortId <= 1)
                    {
                        return JsonError("unavailable to move up");
                    }
                    int? sorttemp = c.SortId;
                    var b = ctx.Menus.Where(x => x.SortId == (sorttemp-1) && x.ApplicationId == menu.ApplicationId && (x.ParentId == c.ParentId) && x.IsDelete == false).FirstOrDefault();
                    c.SortId = b.SortId;
                    b.SortId = sorttemp;
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { message = "succes" });
                }
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> Json_movedownmenu(Navbar menu)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var c = ctx.Menus.Where(x => x.Id == menu.Id && x.IsDelete == false).FirstOrDefault();
                    if (c == null)
                    {
                        return JsonError("Menu tidak ada dalam database");
                    }
                    int? sorttemp = c.SortId;
                    var b = ctx.Menus.Where(x => x.SortId == (sorttemp + 1) && x.ApplicationId == menu.ApplicationId && (x.ParentId == c.ParentId) && x.IsDelete == false).FirstOrDefault();
                    if (b == null)
                    {
                        return JsonError("unavailable to move down");
                    }
                    c.SortId = b.SortId;
                    b.SortId = sorttemp;
                    await ctx.SaveChangesAsync();
                    return JsonSuccess(new { message = "succes" });
                }
            });
        }

        [AllowAnonymous]
        public ActionResult MainMenu(string controller, string action)
        {
            string fullString = string.Empty;

            string[] roles = _currentUser.Roles;

            ViewBag.UserName = _currentUser.Username;
            ViewBag.FullName = _currentUser.User.FullName;

            ApplicationRole adminRole = RoleManager.Roles.FirstOrDefault(r=>r.Name == "Admin");

            IList<Navbar> listOfNodes = null;

            if (adminRole != null && roles.Contains(adminRole.Id)) //if admin role exists in this user show all menu
            {
                listOfNodes = GetListOfNodes(false);
                var topLevelMenus = listOfNodes.GenerateTree(c => c.Id, c => c.ParentId).ToList();
                //IList<Navbar> topLevelMenus = listOfNodes.Where(x => x != null).SelectMany(x => x.Children).ToList(); ;

                foreach (var menu in topLevelMenus.OrderBy(m => m.Item.ApplicationId).ThenBy(n => n.Item.SortId))
                {
                    currentMenuLevel = 1;
                    fullString += EnumerateMainMenuNodes(menu, controller, action);
                }
            }
            else
            {
                //foreach (var roleId in roles)
                //{
                    listOfNodes = GetListOfNodesByRoleAndApplication(roles);
                    //IList<Navbar> topLevelMenus = listOfNodes.SelectMany(x => x.Children).ToList(); ;
                    var topLevelMenus = listOfNodes.GenerateTree(c => c.Id, c => c.ParentId).ToList();
                    foreach (var menu in topLevelMenus.OrderBy(m => m.Item.ApplicationId).ThenBy(n => n.Item.SortId))
                    {
                        currentMenuLevel = 1;
                        fullString += EnumerateMainMenuNodes(menu, controller, action);
                    }
                //}
            }

            fullString += GenerateLogOutLink();
            
            return PartialView("_Navbar", (object)fullString);
        }

        private int currentMenuLevel = 1;

        private string EnumerateMainMenuNodes(TreeItem<Navbar> parent, string controller, string action)
        { 
            // Init an empty string
            string content = String.Empty;

            // Add <li> menu name
            content += "<li>";

            string activeParentMenu = "";
            if (parent.Item.Parent == null)
            {
                //cari semua children dengan controller dan action yg sama
                if (parent.Children
                    .Where(m => m.Item.Parent.Id == parent.Item.Id
                    && m.Item.Controller == controller
                    && m.Item.Action == action).Count() > 0)
                {
                    content += "<li class=\"active\">";
                    activeParentMenu = "class='activeParentMenu'";
                }
                else
                {
                    //jika tidak ada maka loop semua anak dari parent tersebut dan cek jika ada controller dan action yg sama
                    foreach (var child in parent.Children.OrderBy(m => m.Item.SortId))
                    {
                        if (_applicationDbContext.Menus                            
                            .Where(m => m.Parent.Id == child.Item.Id
                            && m.Controller == controller
                            && m.Action == action).Count() > 0)
                        {
                            content += "<li class=\"active\">";
                            activeParentMenu = "class='activeParentMenu'";
                            break;
                        }
                    }
                }

            }            
            
             // If there are no controller>
            if (string.IsNullOrEmpty(parent.Item.Controller))
                content += "<a href=\"#\" " + activeParentMenu + ">";
            else
                content += "<a href=\"" + WebHelpers.BaseUrl() + parent.Item.Controller + "/" + parent.Item.Action + "\" " + activeParentMenu + ">";

            content += "<i class=\"" + (parent.Item.faicon != null ? parent.Item.faicon : (parent.Item.ImageClass != null ? parent.Item.ImageClass.ImageClassName : "")) + "\"></i>";

            if (currentMenuLevel == 1)
            {
                content += "<span class=\"side-menu-title\"> " + parent.Item.Text + "</span>";
            }
            else
            {
                content += "<span> " + parent.Item.Text + "</span>";
            }
          
            // If there are no children, end the </li>
            if (parent.Children.Count() == 0)
            {
 
                content += "</a>";
                content += "</li>";
            }
            else   // If there are children, start a <ul>
            {
                
                currentMenuLevel++;
                
                content += "<span class=\"fa arrow\"></span>";
                content += "</a>";
                content += "<ul class=\"nav " + (currentMenuLevel == 2 ? "nav-second-level" : "nav-third-level") + "\">";
            }

            // Loop one past the number of children
            int numberOfChildren = parent.Children.Count();
            //for (int i = 0; i <= numberOfChildren; i++)
            int i = 0;
            foreach (var child in parent.Children.OrderBy(m => m.Item.SortId))                    
            {
                // If this iteration's index points to a child,
                // call this function recursively
                if (numberOfChildren > 0 && i < numberOfChildren)
                {
                    //Navbar child = parent.Children[i];
                    content += EnumerateMainMenuNodes(child, controller, action);
                }

                i++;

                // If this iteration's index points past the children, end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";                
            }

            // Return the content
            return content;
        }

        private string GenerateLogOutLink()
        {
            string content = "<li>";
            content += "<a href=\"" + WebHelpers.BaseUrl() + "Account/LogOff\">";
            content += "<i class=\"fa fa-sign-out fa-fw\"></i>";
            content += "<span class=\"side-menu-title\">Logout</span>";
            content += "</a>";
            content += "</li>";

            return content;
        }

        private int? GetMaxsort(Guid? parentId, int? AppId) 
        {
            if (AppId == null)
            {
                return null;
            }
            var model = _applicationDbContext.Menus.Where(m => m.ParentId == parentId && m.ApplicationId == AppId).OrderByDescending(m => m.SortId).FirstOrDefault();
            if (model == null)
            {
                return 0;
            }
            return model.SortId;
        }

        private List<Navbar> GetListOfNodes(bool selectAll = true)
        {
            string applicationName = WebConfigurationManager.AppSettings["ApplicationName"];            
            Application app = _applicationDbContext.ApplicationSet.FirstOrDefault(a => a.ApplicationName == applicationName);

            List<Navbar> sourceMenus = _applicationDbContext.Menus.Where(m => m.IsDelete == false && ((!selectAll && m.ApplicationId == app.ApplicationId) || selectAll)).Include("Children").OrderBy(m => m.ApplicationId).ThenBy(n => n.SortId).ToList();
            List<Navbar> menus = new List<Navbar>();
            foreach (Navbar sourceMenu in sourceMenus)
            {
                Navbar c = new Navbar();
                c.Id = sourceMenu.Id;
                c.Name = sourceMenu.Name;
                c.Text = sourceMenu.Text;
                c.Controller = sourceMenu.Controller;
                c.faicon = sourceMenu.faicon;
                c.description = sourceMenu.description;
                c.Action = sourceMenu.Action;
                c.ImageClass = sourceMenu.ImageClass;
                c.SortId = sourceMenu.SortId;
                c.ApplicationId = sourceMenu.ApplicationId;
                c.ParentId = sourceMenu.ParentId;
                if (sourceMenu.ParentId != null)
                {
                    c.Parent = new Navbar();
                    c.Parent.Id = (Guid)sourceMenu.ParentId;
                }
                menus.Add(c);
            }
            return menus;
        }

        private List<Navbar> GetListOfNodesByRoleAndApplication(string[] roleId, string applicationName = "")
        {
            //get app name
            if (string.IsNullOrEmpty(applicationName))
            {
                applicationName = WebConfigurationManager.AppSettings["ApplicationName"];
            }
            Application app = _applicationDbContext.ApplicationSet.FirstOrDefault(a => a.ApplicationName == applicationName);

            List<Navbar> sourceMenus = _applicationDbContext.Menus
                //.Where(m => m.Roles.Count(r => r.Id == roleId) > 0 && m.ApplicationId == app.ApplicationId)
                .Where(m => m.IsDelete == false && m.Roles.Count(r => roleId.Contains(r.Id)) > 0 && m.ApplicationId == app.ApplicationId)
                .DistinctBy(p => p.Id)
                .OrderBy(m => m.ApplicationId).ThenBy(n => n.SortId)
                .ToList();
            List<Navbar> menus = new List<Navbar>();
            foreach (Navbar sourceMenu in sourceMenus)
            {
                Navbar c = new Navbar();
                c.Id = sourceMenu.Id;
                c.Name = sourceMenu.Name;
                c.Text = sourceMenu.Text;
                c.Controller = sourceMenu.Controller;
                c.faicon = sourceMenu.faicon;
                c.description = sourceMenu.description;
                c.Action = sourceMenu.Action;
                c.ImageClass = sourceMenu.ImageClass;
                c.SortId = sourceMenu.SortId;
                c.ApplicationId = sourceMenu.ApplicationId;
                c.ParentId = sourceMenu.ParentId;
                if (sourceMenu.ParentId != null)
                {
                    c.Parent = new Navbar();
                    c.Parent.Id = (Guid)sourceMenu.ParentId;
                }
                menus.Add(c);
            }
            return menus;
        }

        private List<Navbar> GetListOfNodesByApplication(string applicationName = "")
        {
            //get app name
            if (string.IsNullOrEmpty(applicationName))
            {
                applicationName = WebConfigurationManager.AppSettings["ApplicationName"];
            }
            Application app = _applicationDbContext.ApplicationSet.FirstOrDefault(a => a.ApplicationName == applicationName);

            List<Navbar> sourceMenus = _applicationDbContext.Menus
                .Where(m => m.IsDelete == false && m.ApplicationId == app.ApplicationId)
                .DistinctBy(p => p.Id)
                .OrderBy(m => m.ApplicationId).ThenBy(n => n.SortId)
                .ToList();
            List<Navbar> menus = new List<Navbar>();
            foreach (Navbar sourceMenu in sourceMenus)
            {
                Navbar c = new Navbar();
                c.Id = sourceMenu.Id;
                c.Name = sourceMenu.Name;
                c.Text = sourceMenu.Text;
                c.Controller = sourceMenu.Controller;
                c.Action = sourceMenu.Action;
                c.ImageClass = sourceMenu.ImageClass;
                c.SortId = sourceMenu.SortId;
                c.faicon = sourceMenu.faicon;
                c.description = sourceMenu.description;
                c.ApplicationId = sourceMenu.ApplicationId;
                c.ParentId = sourceMenu.ParentId;
                if (sourceMenu.ParentId != null)
                {
                    c.Parent = new Navbar();
                    c.Parent.Id = (Guid)sourceMenu.ParentId;
                }
                menus.Add(c);
            }
            return menus;
        }

        private string GetMenuRoleActionLinks(int menuId, string roleId)
        {
            string content = String.Empty;
            content += String.Format("<a href=\"{0}MenuRole/DeleteMenuFromRole/{1}/{2}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete From Role</a>", WebHelpers.BaseUrl(), menuId, roleId);
            return content;
        }

        private string GetMenuMasterActionLinks(int parentId)
        {
            string content = String.Empty;
            content += String.Format("<a href=\"{0}Navbar/Edit/{1}\" class=\"btn btn-primary btn-xs treenodeeditbutton\">Edit</a>", WebHelpers.BaseUrl(),parentId);
            content += String.Format("<a href=\"{0}Navbar/Delete/{1}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete</a>", WebHelpers.BaseUrl(),parentId);
            return content; 
        }

        private string EnumerateNodesMenuMaster(Navbar parent)
        {
            // Init an empty string
            string content = String.Empty;
            int? maxsort = GetMaxsort(parent.ParentId, parent.ApplicationId);
            // Add <li> category name
            content += "<li class=\"treenode\">";
            content += parent.SortId.ToString()+ " - "+parent.Name;

            content += String.Format("<a href=\"{0}Navbar/Edit/{1}\" class=\"btn btn-primary btn-xs treenodeeditbutton\">Edit</a>", WebHelpers.BaseUrl(), parent.Id);
            content += String.Format("<a href=\"{0}Navbar/Delete/{1}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete</a>", WebHelpers.BaseUrl(), parent.Id);

            content += String.Format("  <div class=\"btn-group btn-group-sm\" role=\"group\" aria-label=\"...\">" +
                "<a href=\"{0}Navbar/MoveUp/{1}\" class=\"btn btn-default {2}\" aria-label=\"Left Align\"><span class=\"glyphicon glyphicon-hand-up\" aria-hidden=\"true\" ></span></a>" +
                "<a href=\"{0}Navbar/MoveDown/{1}\" class=\"btn btn-default {3}\" aria-label=\"Left Align\"><span class=\"glyphicon glyphicon-hand-down\" aria-hidden=\"true\"></span></a>" +
                "</div>", WebHelpers.BaseUrl(), parent.Id, parent.SortId == 1 ? "disabled" : "", parent.SortId == maxsort ? "disabled" : "");
            // If there are no children, end the </li>
            if (parent.Children.Count == 0)
                content += "</li>";
            else   // If there are children, start a <ul>
                content += "<ul>";

            // Loop one past the number of children
            int numberOfChildren = parent.Children.Count;
            for (int i = 0; i <= numberOfChildren; i++)
            {
                // If this iteration's index points to a child,
                // call this function recursively
                if (numberOfChildren > 0 && i < numberOfChildren)
                {
                    Navbar child = parent.Children[i];
                    content += EnumerateNodesMenuMaster(child);
                    //content += EnumerateNodes(child);
                }

                // If this iteration's index points past the children, end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";
            }

            // Return the content
            return content;
        }

        private string EnumerateNodesMenuRole(Navbar parent, string roleId)
        {
            // Init an empty string
            string content = String.Empty;

            // Add <li> category name
            content += "<li class=\"treenode\">";
            content += parent.Name;

            //content += String.Format("<a href=\"/MenuRole/DeleteMenuFromRole/{0}/{1}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete From Role</a>", parent.Id, roleId);
            content += String.Format("<a href=\"{0}MenuRole/DeleteMenuFromRole/{1}/{2}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete From Role</a>", WebHelpers.BaseUrl(), parent.Id, roleId);
            
            // If there are no children, end the </li>
            if (parent.Children.Count == 0)
                content += "</li>";
            else   // If there are children, start a <ul>
                content += "<ul>";

            // Loop one past the number of children
            int numberOfChildren = parent.Children.Count;
            for (int i = 0; i <= numberOfChildren; i++)
            {
                // If this iteration's index points to a child,
                // call this function recursively
                if (numberOfChildren > 0 && i < numberOfChildren)
                {
                    Navbar child = parent.Children[i];
                    content += EnumerateNodesMenuRole(child, roleId);                    
                }

                // If this iteration's index points past the children, end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";
            }

            // Return the content
            return content;
        }

        private string EnumerateNodes(Navbar parent)
        {
            // Init an empty string
            string content = String.Empty;

            // Add <li> category name
            content += "<li class=\"treenode\">";
            content += parent.Name;
            
            //content += String.Format("<a href=\"/Navbar/Edit/{0}\" class=\"btn btn-primary btn-xs treenodeeditbutton\">Edit</a>", parent.Id);
            //content += String.Format("<a href=\"/Navbar/Delete/{0}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete</a>", parent.Id);

            content += String.Format("<a href=\"{0}Navbar/Edit/{1}\" class=\"btn btn-primary btn-xs treenodeeditbutton\">Edit</a>", WebHelpers.BaseUrl(), parent.Id);
            content += String.Format("<a href=\"{0}Navbar/Delete/{1}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete</a>", WebHelpers.BaseUrl(), parent.Id);

            // If there are no children, end the </li>
            if (parent.Children.Count == 0)
                content += "</li>";
            else   // If there are children, start a <ul>
                content += "<ul>";

            // Loop one past the number of children
            int numberOfChildren = parent.Children.Count;
            for (int i = 0; i <= numberOfChildren; i++)
            {
                // If this iteration's index points to a child,
                // call this function recursively
                if (numberOfChildren > 0 && i < numberOfChildren)
                {
                    Navbar child = parent.Children[i];
                    content += EnumerateNodes(child);
                }

                // If this iteration's index points past the children, end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";
            }

            // Return the content
            return content;
        }

        private void ValidateParentsAreParentless(Navbar menu)
        {
            // There is no parent
            if (menu.ParentId == null)
                return;

            // The parent has a parent
            Navbar parentMenu = _applicationDbContext.Menus.Find(menu.ParentId);
            if (parentMenu.ParentId != null)
            {
                //throw new InvalidOperationException("You cannot nest this category more than two levels deep.");
                Navbar parentMenu2 = _applicationDbContext.Menus.Find(parentMenu.ParentId);
                if (parentMenu2.ParentId != null)
                {
                    throw new InvalidOperationException("You cannot nest this category more than three levels deep.");
                }
            }

            // The parent does NOT have a parent, but the category being nested has children
            //int numberOfChildren = _applicationDbContext.Menus.Count(c => c.ParentId == menu.Id);
            //if (numberOfChildren > 0)
            //    throw new InvalidOperationException("You cannot nest this category's children more than two levels deep.");
        }

        private SelectList PopulateParentMenuSelectList(Guid? id)
        {
            SelectList selectList;

            if (id == null)
                selectList = new SelectList(
                    _applicationDbContext
                    .Menus
                    .OrderBy(m => m.Name)
                    //.Where(c => c.ParentId == null || c.Children.Count > 0), "Id", "Name");
                    .Where(c => c.ParentId == null || c.Parent.ParentId == null), "Id", "Name");
            else //if (_applicationDbContext.Menus.Count(c => c.ParentId == id) == 0)
                selectList = new SelectList(
                    _applicationDbContext
                    .Menus
                    .OrderBy(m => m.Name)
                    //.Where(c => (c.ParentId == null || c.Children.Count > 0) && c.Id != id), "Id", "Name");
                    .Where(c => (c.ParentId == null || c.Parent.ParentId == null) && c.Id != id), "Id", "Name");
            return selectList;
        }

        private SelectList PopulateImageClassSelectList()
        {
            return new SelectList(
                    _applicationDbContext
                    .ImageClassSet
                    .ToList(), "ImageClassId", "ImageClassName"); 
        }

        private SelectList PopulateApplicationIdSelectList()
        {
            return new SelectList(
                    _applicationDbContext
                    .ApplicationSet
                    .ToList(), "ApplicationId", "ApplicationName");
        }

        public async Task<ActionResult> Index()
        {
            //// Start the outermost list
            //string fullString = "<ul>";

            //IList<Navbar> listOfNodes = GetListOfNodes();
            //IList<Navbar> topLevelCategories = listOfNodes.SelectMany(x => x.Children).ToList(); ;

            //foreach (var category in topLevelCategories)
            //    fullString += EnumerateNodesMenuMaster(category);

            //// End the outermost list
            //fullString += "</ul>";

            return View();
        }

        //menu role controller tree view
        public async Task<string> GetMenusByRole(string roleId, string applicationName = "")
        {
            // Start the outermost list
            string fullString = "<ul>";

            IList<Navbar> listOfNodes = GetListOfNodesByRoleAndApplication(new string[]{ roleId }, applicationName);
            IList<Navbar> topLevelCategories = listOfNodes.SelectMany(x => x.Children).ToList();

            foreach (var category in topLevelCategories)
                fullString += EnumerateNodesMenuRole(category, roleId);

            // End the outermost list
            fullString += "</ul>";

            return fullString;
        }

        public ActionResult Create()
        {
            ViewBag.ParentIdSelectList = PopulateParentMenuSelectList(null);
            ViewBag.ImageClassIdSelectList = PopulateImageClassSelectList();
            ViewBag.ApplicationIdSelectList = PopulateApplicationIdSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ParentId,Name,Text,Controller,Action,ImageClassId,ApplicationId")] Navbar menu)
        {
            ViewBag.ParentIdSelectList = PopulateParentMenuSelectList(null);
            ViewBag.ImageClassIdSelectList = PopulateImageClassSelectList();
            ViewBag.ApplicationIdSelectList = PopulateApplicationIdSelectList();

            if (ModelState.IsValid)
            {
                try
                {
                    ValidateParentsAreParentless(menu);
                    int? maxsort = GetMaxsort(menu.ParentId, menu.ApplicationId);
                    menu.SortId = maxsort == null ? 1 : maxsort + 1 ;
                    var existingModel = _applicationDbContext.Menus.FirstOrDefault(c => c.Name.ToLower() == menu.Name.ToLower());
                    
                    if(existingModel != null)
                        throw new Exception("Nama Menu sudah ada di database!");

                    _applicationDbContext.Menus.Add(menu);
                    await _applicationDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);                    
                    ViewBag.ParentCategoryIdSelectList = PopulateParentMenuSelectList(null);
                    return View(menu);
                }
                
                return RedirectToAction("Index");
            }
            
            return View(menu);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Navbar menu = await _applicationDbContext.Menus.FindAsync(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            // Wind-up a Navbar viewmodel
            NavbarViewModel navbarViewModel = new NavbarViewModel();
            navbarViewModel.Id = menu.Id;
            navbarViewModel.ParentId = menu.ParentId;
            navbarViewModel.Name = menu.Name;
            navbarViewModel.Text = menu.Text;
            navbarViewModel.Controller = menu.Controller;
            navbarViewModel.Action = menu.Action;
            navbarViewModel.ImageClassId = menu.ImageClassId;
            navbarViewModel.ApplicationId = menu.ApplicationId;
            navbarViewModel.SortId = menu.SortId;

            ViewBag.ParentIdSelectList = PopulateParentMenuSelectList(navbarViewModel.Id);
            ViewBag.ImageClassIdSelectList = PopulateImageClassSelectList();
            ViewBag.ApplicationIdSelectList = PopulateApplicationIdSelectList();
            return View(navbarViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParentId,Name,Text,Controller,Action,ImageClassId,ApplicationId,SortId")] NavbarViewModel navbarViewModel)
        {
            ViewBag.ParentIdSelectList = PopulateParentMenuSelectList(navbarViewModel.Id);
            ViewBag.ImageClassIdSelectList = PopulateImageClassSelectList();
            ViewBag.ApplicationIdSelectList = PopulateApplicationIdSelectList();

            if (ModelState.IsValid)
            {
                // Unwind back to a Navbar
                Navbar editedNavbar = new Navbar();

                try
                {
                    editedNavbar.Id = navbarViewModel.Id;
                    editedNavbar.ParentId = navbarViewModel.ParentId;
                    editedNavbar.Name = navbarViewModel.Name;
                    editedNavbar.Text = navbarViewModel.Text;
                    editedNavbar.Controller = navbarViewModel.Controller;
                    editedNavbar.Action = navbarViewModel.Action;
                    editedNavbar.ImageClassId = navbarViewModel.ImageClassId;
                    editedNavbar.ApplicationId = navbarViewModel.ApplicationId;
                    editedNavbar.SortId = navbarViewModel.SortId;
                    ValidateParentsAreParentless(editedNavbar);
                    _applicationDbContext.Entry(editedNavbar).State = EntityState.Modified;
                    await _applicationDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);                   
                                        
                    ViewBag.ParentIdSelectList = PopulateParentMenuSelectList(navbarViewModel.Id);
                    ViewBag.ImageClassIdSelectList = PopulateImageClassSelectList();
                    ViewBag.ApplicationIdSelectList = PopulateApplicationIdSelectList();
                    return View("Edit", navbarViewModel);
                }
               
                return RedirectToAction("Index");
            }
            
            return View(navbarViewModel);
        }

        public async Task<ActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Navbar menu = await _applicationDbContext.Menus.FindAsync(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        public async Task<ActionResult> MoveUp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var target = await _applicationDbContext.Menus.FindAsync(id);
            if (target == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var target2 = _applicationDbContext.Menus.Where(m => m.ParentId == target.ParentId && m.ApplicationId == target.ApplicationId && m.SortId == (target.SortId - 1)).FirstOrDefault();
            int? temp = target.SortId;
            target.SortId = target2.SortId;
            target2.SortId = temp;
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> MoveDown(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var target = await _applicationDbContext.Menus.FindAsync(id);
            if (target == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var target2 = _applicationDbContext.Menus.Where(m => m.ParentId == target.ParentId && m.ApplicationId == target.ApplicationId && m.SortId == (target.SortId + 1)).FirstOrDefault();
            int? temp = target.SortId;
            target.SortId = target2.SortId;
            target2.SortId = temp;
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Navbar menu = await _applicationDbContext.Menus.FindAsync(id);

            try
            {
                _applicationDbContext.Menus.Remove(menu);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "You attempted to delete a menu that had menus associated with it.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Delete", menu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _applicationDbContext.Dispose();                
            }
            base.Dispose(disposing);
        }

    }
}