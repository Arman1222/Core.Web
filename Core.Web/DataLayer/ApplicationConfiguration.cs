namespace MyWeb.Migrations
{
    using Core.Web.Models.Applications;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MyWeb.DataLayer;
    using MyWeb.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    //AddColumn("dbo.UserMaster", "Id", c => c.String(nullable: false, maxLength: 128));
    //        AddColumn("dbo.UserMaster", "FirstName", c => c.String(maxLength: 15));
    //        AddColumn("dbo.UserMaster", "LastName", c => c.String(maxLength: 15));
    //        AddColumn("dbo.UserMaster", "Email", c => c.String(maxLength: 256));
    //        AddColumn("dbo.UserMaster", "EmailConfirmed", c => c.Boolean(nullable: false));
    //        AddColumn("dbo.UserMaster", "PasswordHash", c => c.String());
    //        AddColumn("dbo.UserMaster", "SecurityStamp", c => c.String());
    //        AddColumn("dbo.UserMaster", "PhoneNumber", c => c.String());
    //        AddColumn("dbo.UserMaster", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
    //        AddColumn("dbo.UserMaster", "TwoFactorEnabled", c => c.Boolean(nullable: false));
    //        AddColumn("dbo.UserMaster", "LockoutEndDateUtc", c => c.DateTime());
    //        AddColumn("dbo.UserMaster", "LockoutEnabled", c => c.Boolean(nullable: false));
    //        AddPrimaryKey("dbo.UserMaster", "Id");
    //        CreateIndex("dbo.UserMaster", "UserName", unique: true, name: "UserNameIndex");
    internal sealed class ApplicationConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public ApplicationConfiguration()
        {      
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Application";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //http://stackoverflow.com/questions/17169020/debug-code-first-entity-framework-migration-codes
            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();

            var userManager =
               new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                //http://stackoverflow.com/questions/21057304/why-is-usermanager-find-returning-null-if-my-username-is-email-based
                AllowOnlyAlphanumericUserNames = false
            };

            var roleManager =
                new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));

            //create admin
            string name = "steven";
            string password = "111111";
            string firstName = "Admin";
            string roleName = "Admin";

            var role = roleManager.FindByName(roleName);

            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleResult = roleManager.Create(role);
            }

            var userAdmin = userManager.FindByName(name);

            if (userAdmin == null)
            {
                userAdmin = new ApplicationUser { UserName = name, Email = name, FirstName = firstName, EmailConfirmed = true, NIK = "111111",LockoutEnabled = false };
                var result = userManager.Create(userAdmin, password);
                result = userManager.SetLockoutEnabled(userAdmin.Id, false);
            }           

            var rolesForUser = userManager.GetRoles(userAdmin.Id);

            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(userAdmin.Id, role.Name);
            }

            //create user 
            name = "guest";
            password = "111111";
            firstName = "Guest";
            roleName = "User";

            role = roleManager.FindByName(roleName);

            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleResult = roleManager.Create(role);
            }

            var userGuest = userManager.FindByName(name);

            if (userGuest == null)
            {
                userGuest = new ApplicationUser { UserName = name, Email = name, FirstName = firstName, EmailConfirmed = true, NIK = "222222", LockoutEnabled = false };
                var result = userManager.Create(userGuest, password);                
                result = userManager.SetLockoutEnabled(userGuest.Id, false); 
            }

            rolesForUser = userManager.GetRoles(userGuest.Id);

            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(userGuest.Id, role.Name);
            }

            ////////////////////////////////////////////////////
            //APPLICATION
            context.ApplicationSet.AddOrUpdate(
                       i => i.ApplicationName
                       , new Application { ApplicationName = "MyWeb", CreateBy = userAdmin.Id }
                   );
            context.SaveChanges();

            var applicationMyWeb = context.ApplicationSet.FirstOrDefault (i => i.ApplicationName == "MyWeb");
            

            //COMPANY
            context.CompanySet.AddOrUpdate(
                        i => i.CompanyName
                        , new Company { CompanyName = "Mayora", CreateBy = userAdmin.Id }
                    );
            context.SaveChanges();

            var companyMayora = context.CompanySet.FirstOrDefault(i => i.CompanyName == "Mayora");         

            //BRANCH            
            var branchPusat = context.BranchSet.FirstOrDefault(i => i.BranchName == "KC TOMANG");
            
            //set branch
            if (branchPusat != null)
            {
                userAdmin.BranchId = branchPusat.BranchId;
                userGuest.BranchId = branchPusat.BranchId;
            }
            //set company
            if (companyMayora != null)
            {
                userAdmin.CompanyId = companyMayora.CompanyId;
                userGuest.CompanyId = companyMayora.CompanyId;
            }

            userManager.Update(userAdmin);
            userManager.Update(userGuest);

            //IMAGE CLASS
            context.ImageClassSet.AddOrUpdate(
                    i=>i.ImageClassName
                    , new ImageClass { ImageClassName = "fa fa-dashboard fa-fw" }
                );
            context.SaveChanges();

            context.ImageClassSet.AddOrUpdate(
                    i => i.ImageClassName
                    , new ImageClass { ImageClassName = "fa fa-bar-chart-o fa-fw" }
                );
            context.SaveChanges();

            context.ImageClassSet.AddOrUpdate(
                    i => i.ImageClassName
                    , new ImageClass { ImageClassName = "fa fa-table fa-fw" }
                );
            context.SaveChanges();

            context.ImageClassSet.AddOrUpdate(
                    i => i.ImageClassName
                    , new ImageClass { ImageClassName = "fa fa-edit fa-fw" }
                );
            context.SaveChanges();

            context.ImageClassSet.AddOrUpdate(
                    i => i.ImageClassName
                    , new ImageClass { ImageClassName = "fa fa-wrench fa-fw" }
                );
            context.SaveChanges();

            context.ImageClassSet.AddOrUpdate(
                    i => i.ImageClassName
                    , new ImageClass { ImageClassName = "fa fa-sitemap fa-fw" }
                );
            context.SaveChanges();

            context.ImageClassSet.AddOrUpdate(
                    i => i.ImageClassName
                    , new ImageClass { ImageClassName = "fa fa-files-o fa-fw" }
                );
            context.SaveChanges();

            ////////////////////////////////////////////////////

            var imageClassDashboard = context.ImageClassSet.First(i=>i.ImageClassName =="fa fa-dashboard fa-fw");
            var imageClassChart = context.ImageClassSet.First(i => i.ImageClassName == "fa fa-bar-chart-o fa-fw");
            var imageClassTable = context.ImageClassSet.First(i => i.ImageClassName == "fa fa-table fa-fw");
            var imageClassEdit = context.ImageClassSet.First(i => i.ImageClassName == "fa fa-edit fa-fw");
            var imageClassWrench = context.ImageClassSet.First(i => i.ImageClassName == "fa fa-wrench fa-fw");
            var imageClassSiteMap = context.ImageClassSet.First(i => i.ImageClassName == "fa fa-sitemap fa-fw");
            var imageClassFiles = context.ImageClassSet.First(i => i.ImageClassName == "fa fa-files-o fa-fw");

            //menu dashboard
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Dashboard", Text = "Dashboard", Controller = "Home", Action = "Index", ImageClassId = imageClassDashboard.ImageClassId, Status = true, IsParent = true, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example", Text = "Example", ImageClassId = imageClassDashboard.ImageClassId, Status = true, IsParent = true, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            Navbar menuExample = context.Menus.First(c => c.Name == "Example");

            //menu Example-Customer
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Customer", Text = "Customer", Controller = "Customer", Action = "Index", ImageClass = imageClassChart, Status = true, IsParent = true, ParentId = menuExample.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-Charts
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Charts", Text = "Charts", ImageClass = imageClassChart, Status = true, IsParent = true, ParentId = menuExample.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            Navbar menuCharts = context.Menus.First(c => c.Name == "Example-Charts");

            //menu Example-Charts-FlotCharts
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Charts-FlotCharts", Text = "Flot Charts", Controller = "Home", Action = "FlotCharts", ImageClassId = imageClassChart.ImageClassId, Status = true, IsParent = false, ParentId = menuCharts.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-Charts-MorrisJsCharts
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Charts-MorrisJsCharts", Text = "Morris.js Charts", Controller = "Home", Action = "MorrisCharts", ImageClassId = imageClassChart.ImageClassId, Status = true, IsParent = false, ParentId = menuCharts.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-Tables
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Tables", Text = "Tables", Controller = "Home", Action = "Tables", ImageClassId = imageClassTable.ImageClassId, Status = true, IsParent = false, ParentId = menuExample.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-Forms
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Forms", Text = "Forms", Controller = "Home", Action = "Forms", ImageClassId = imageClassEdit.ImageClassId, Status = true, IsParent = false, ParentId = menuExample.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-UIElements
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-UIElements", Text = "UI Elements", ImageClassId = imageClassWrench.ImageClassId, Status = true, IsParent = true, ParentId = menuExample.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            Navbar menuUIElements = context.Menus.First(c => c.Name == "Example-UIElements");

            //menu Example-PanelsAndWells
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-PanelsAndWells", Text = "Panels and Wells", Controller = "Home", Action = "Panels", ImageClassId = imageClassWrench.ImageClassId, Status = true, IsParent = false, ParentId = menuUIElements.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-Buttons
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Buttons", Text = "Buttons", Controller = "Home", Action = "Buttons", ImageClassId = imageClassWrench.ImageClassId, Status = true, IsParent = false, ParentId = menuUIElements.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-Notifications
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Notifications", Text = "Notifications", Controller = "Home", Action = "Notifications", ImageClassId = imageClassWrench.ImageClassId, Status = true, IsParent = false, ParentId = menuUIElements.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-Typography
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Typography", Text = "Typography", Controller = "Home", Action = "Typography", ImageClassId = imageClassWrench.ImageClassId, Status = true, IsParent = false, ParentId = menuUIElements.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-Icons
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Icons", Text = "Icons", Controller = "Home", Action = "Icons", ImageClassId = imageClassWrench.ImageClassId, Status = true, IsParent = false, ParentId = menuUIElements.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-Grid
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-Grid", Text = "Grid", Controller = "Home", Action = "Grid", ImageClassId = imageClassWrench.ImageClassId, Status = true, IsParent = false, ParentId = menuUIElements.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-MultiLevelDropdown
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-MultiLevelDropdown", Text = "Multi Level Dropdown", ImageClassId = imageClassSiteMap.ImageClassId, Status = true, IsParent = true, ParentId = menuExample.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            Navbar menuMultiLevelDropdown = context.Menus.First(c => c.Name == "Example-MultiLevelDropdown");

            //menu Example-SecondLevelItem
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-SecondLevelItem", Text = "Second Level Item", ImageClassId = imageClassWrench.ImageClassId, Status = true, IsParent = false, ParentId = menuMultiLevelDropdown.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-SamplePages
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-SamplePages", Text = "Sample Pages", ImageClassId = imageClassFiles.ImageClassId, Status = true, IsParent = true, ParentId = menuExample.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            Navbar menuSamplePages = context.Menus.First(c => c.Name == "Example-SamplePages");

            //menu Example-BlankPage
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-BlankPage", Text = "Blank Page", Controller = "Home", Action = "Blank", ImageClassId = imageClassFiles.ImageClassId, Status = true, IsParent = false, ParentId = menuSamplePages.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            //menu Example-LoginPage
            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "Example-LoginPage", Text = "Login Page", Controller = "Home", Action = "Login", ImageClassId = imageClassFiles.ImageClassId, Status = true, IsParent = false, ParentId = menuSamplePages.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            string menuName = "User-Menu";

            context.Menus.AddOrUpdate(
                m => m.Name,
                new Navbar { Name = "User", Text = "User", ImageClassId = imageClassDashboard.ImageClassId, Status = true, IsParent = true, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            Navbar menuUser = context.Menus.First(c => c.Name == "User");

            context.Menus.AddOrUpdate(
             m => m.Name,
             new Navbar { Name = "User-UserMaster", Text = "User Master", Controller = "ApplicationUsers", Action = "Index", ImageClassId = imageClassDashboard.ImageClassId, Status = true, IsParent = false, ParentId = menuUser.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            context.Menus.AddOrUpdate(
               m => m.Name,
               new Navbar { Name = "User-Role", Text = "Role Master", Controller = "ApplicationRoles", Action = "Index", ImageClassId = imageClassDashboard.ImageClassId, Status = true, IsParent = false, ParentId = menuUser.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            context.Menus.AddOrUpdate(
             m => m.Name,
             new Navbar { Name = "User-MenuMaster", Text = "Menu Master", Controller = "Navbar", Action = "Index", ImageClassId = imageClassDashboard.ImageClassId, Status = true, IsParent = false, ParentId = menuUser.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            context.Menus.AddOrUpdate(
             m => m.Name,
             new Navbar { Name = "User-MenuRole", Text = "Menu Role", Controller = "MenuRole", Action = "Index", ImageClassId = imageClassDashboard.ImageClassId, Status = true, IsParent = false, ParentId = menuUser.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

            context.Menus.AddOrUpdate(
               m => m.Name,
               new Navbar { Name = "Menu-ImageClass", Text = "Image Class", Controller = "ImageClass", Action = "Index", ImageClassId = imageClassDashboard.ImageClassId, Status = true, IsParent = false, ParentId = menuUser.Id, ApplicationId = applicationMyWeb.ApplicationId, CreateBy = userAdmin.Id });

            context.SaveChanges();

        }

    }
}
