using Core.Web.Models;
using Core.Web.Models.Applications;
using Microsoft.AspNet.Identity.EntityFramework;
using MyWeb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace MyWeb.DataLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //InitializeDatabase();
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }

        //public DbSet<InventoryItem> InventoryItems { get; set; }
        //public DbSet<Labor> Labors { get; set; }
        //public DbSet<Part> Parts { get; set; }
        //public DbSet<ServiceItem> ServiceItems { get; set; }
        //public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<Navbar> Menus { get; set; }
        public DbSet<ImageClass> ImageClassSet { get; set; }
        public DbSet<Area> AreaSet { get; set; }
        public DbSet<Branch> BranchSet { get; set; }
        public DbSet<HomeMenuRole> HomeMenuRoleSet { get; set; }
        public DbSet<Company> CompanySet { get; set; }
        public DbSet<Application> ApplicationSet { get; set; }
        public DbSet<MyNote> MyNoteSet { get; set; }
        public DbSet<MyTask> MyTaskSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryConfiguration());

            //modelBuilder.Configurations.Add(new InventoryItemConfiguration());
            //modelBuilder.Configurations.Add(new LaborConfiguration());
            //modelBuilder.Configurations.Add(new PartConfiguration());
            //modelBuilder.Configurations.Add(new ServiceItemConfiguration());
            //modelBuilder.Configurations.Add(new WorkOrderConfiguration());
            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            modelBuilder.Configurations.Add(new MenuConfiguration());
            modelBuilder.Configurations.Add(new ApplicationMasterConfiguration());
            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new BranchConfiguration());
            modelBuilder.Configurations.Add(new AreaConfiguration());
            modelBuilder.Configurations.Add(new MyTaskConfiguration());
            modelBuilder.Configurations.Add(new MyNoteConfiguration());
            modelBuilder.Configurations.Add(new HomeMenuRoleConfiguration());
            //modelBuilder.Configurations.Add(new ApplicationRoleConfiguration());

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<IdentityUser>().ToTable("UserMaster");//.Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<ApplicationUser>().ToTable("UserMaster");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.AccessFailedCount).HasColumnName("PassWrongCount");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.FullName).HasColumnName("UserRealName");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.FirstName).HasColumnName("FirstName");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.LastName).HasColumnName("LastName");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.ExpireDate).HasColumnName("ExpireDate");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.ActiveBit).HasColumnName("ActiveBit");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.PassMaxWrong).HasColumnName("PassMaxWrong");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.LastLogin).HasColumnName("LastLogin");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("RoleMaster");
        }

        //protected virtual void InitializeDatabase()
        //{
        //    if (!Database.Exists())
        //    {
        //        Database.Initialize(true);
        //        new Database.Seed(this);
        //    }
        //}

        public virtual ObjectResult<Application> Sp_get_application_by_userId(string userId)
        {
            var Parameter = userId != null ?
                new SqlParameter("UserId", userId) :
                new SqlParameter("UserId", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<Application>("[_Sp_get_application_by_userId] @UserId", Parameter);
        }

        public virtual ObjectResult<Sp_get_application_by_role_Result> Sp_get_application_by_role(string roleId)
        {
            var roleIdParameter = roleId != null ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<Sp_get_application_by_role_Result>("[_Sp_get_application_by_role] @RoleId", roleIdParameter);
        }

        public virtual ObjectResult<Sp_get_navbar_by_role_app_Result> Sp_get_navbar_by_role_app(string roleId, Nullable<int> applicationId)
        {
            var roleIdParameter = roleId != null ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(string));

            var applicationIdParameter = applicationId.HasValue ?
                new SqlParameter("ApplicationId", applicationId) :
                new SqlParameter("ApplicationId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<Sp_get_navbar_by_role_app_Result>("[_Sp_get_navbar_by_role_app] @RoleId, @ApplicationId", roleIdParameter, applicationIdParameter);
        }

        public virtual void sp_insert_menurole(string roleId, Nullable<System.Guid> navbarId)
        {
            var roleIdParameter = roleId != null ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(string));

            var navbarIdParameter = navbarId.HasValue ?
                new SqlParameter("NavbarId", navbarId) :
                new SqlParameter("NavbarId", typeof(System.Guid));
            ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("[_sp_insert_menurole] @RoleId, @NavbarId", roleIdParameter, navbarIdParameter);
        }

        public virtual void sp_delete_menurole(string roleId, Nullable<System.Guid> navbarId)
        {
            var roleIdParameter = roleId != null ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(string));

            var navbarIdParameter = navbarId.HasValue ?
                new SqlParameter("NavbarId", navbarId) :
                new SqlParameter("NavbarId", typeof(System.Guid));
            ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("[_sp_delete_menurole] @RoleId, @NavbarId", roleIdParameter, navbarIdParameter);
        }


        public virtual ObjectResult<Role_by_user_Result> Sp_get_role_by_user(string UserId)
        {
            var userIdParameter = UserId != null ?
                new SqlParameter("UserId", UserId) :
                new SqlParameter("UserId", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<Role_by_user_Result>("[_Sp_get_role_by_user] @UserId", userIdParameter);
        }

        public virtual void sp_insert_userrole(string userId, string roleId)
        {

            var userIdParameter = userId != null ?
                new SqlParameter("UserId", userId) :
                new SqlParameter("UserId", typeof(string));

            var roleIdParameter = roleId != null ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(string));
            ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("[_sp_insert_userrole] @UserId, @RoleId", userIdParameter, roleIdParameter);
        }

        public virtual void sp_delete_userrole(string userId, string roleId)
        {
            var userIdParameter = userId != null ?
                new SqlParameter("UserId", userId) :
                new SqlParameter("UserId", typeof(string));

            var roleIdParameter = roleId != null ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(string));

            ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("[_sp_delete_userrole] @UserId, @RoleId", userIdParameter, roleIdParameter);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        internal List<JToken> ExecSp(RequestExecSp req, out List<MyParameter> params_, out string ErrMsg, string userId = null)
        {
            using (SqlConnection sqlConx = new SqlConnection(this.Database.Connection.ConnectionString))
            {
                params_ = new List<MyParameter>();
                List<JToken> result = new List<JToken>();
                SqlCommand cmdText = new SqlCommand(req.nmModule + ".dbo." + req.namaSP, sqlConx);
                cmdText.CommandType = CommandType.StoredProcedure;

                if (!String.IsNullOrWhiteSpace(userId))
                {
                    cmdText.Parameters.Add(new SqlParameter("@pActionBy", userId));
                }
                if (req.params_ != null)
                {
                    foreach (var item in req.params_)
                    {
                        SqlParameter Sqlparam = new SqlParameter("@" + item.name, item.dbType);
                        if (item.size != null && item.size > 0)
                            Sqlparam.Size = (int)item.size;
                        //20190723, jeni, begin
                        //Sqlparam.Value = item.value;
                        Sqlparam.Value = item.value == null ? DBNull.Value : item.value;
                        //20190723, jeni, end
                        Sqlparam.Direction = item.prmType;
                        cmdText.Parameters.Add(Sqlparam);
                    }
                }
                #region SQL Input Parameter


                #endregion

                #region SQL Output Parameters

                SqlParameter pErrMsg = new SqlParameter("@pErrMsg", SqlDbType.VarChar, 200);
                pErrMsg.Direction = ParameterDirection.Output;
                cmdText.Parameters.Add(pErrMsg);
                #endregion

                sqlConx.Open();
                SqlDataAdapter theDataAdapter = new SqlDataAdapter(cmdText);
                DataSet ds = new DataSet();
                theDataAdapter.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataTable item in ds.Tables)
                    {
                        //20190514, jeni, begin
                        //if (item.Rows.Count > 0)
                        //{
                        //20190514, jeni, begin
                            result.Add(JToken.Parse(JsonConvert.SerializeObject(item)));
                        //20190514, jeni, begin
                        //}
                        //20190514, jeni, begin
                    }
                }

                #region Assign Properties
                ErrMsg = cmdText.Parameters["@pErrMsg"].Value.ToString();
                foreach (var item in req.params_)
                {
                    if (item.prmType == ParameterDirection.Output || item.prmType == ParameterDirection.InputOutput)
                    {
                        MyParameter newParam = item;
                        newParam.value = cmdText.Parameters["@" + item.name].Value;
                        params_.Add(newParam);
                    }
                }
                ErrMsg = cmdText.Parameters["@pErrMsg"].Value.ToString();
                #endregion

                sqlConx.Close();
                return result;
            }
        }
    }

    public class MyParameter
    {
        public MyParameter()
        {
            this.dbType = System.Data.SqlDbType.VarChar;
            this.prmType = ParameterDirection.Input;
        }
        public string name { get; set; }
        public object value { get; set; }//value
        public SqlDbType dbType { get; set; }
        public ParameterDirection prmType { get; set; }// input output, inputoutput
        public int? size { get; set; }
    }

    public class RequestExecSp
    {
        public RequestExecSp()
        {
            this.pActionBy = true;
        }
        public bool pActionBy { get; set; }
        public string nmModule { get; set; }
        public string namaSP { get; set; }
        public List<MyParameter> params_ { get; set; }
    }
}
