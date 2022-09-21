using MyWeb.ViewModels.area_master;
using MyWeb.ViewModels.euser;
using MyWeb.ViewModels.office_master;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace MyWeb.Migrations.euser
{
    public class SqlUserDbContext : DbContext
    {
        public SqlUserDbContext()
            : base("UserConnection")
        {
            Database.SetInitializer<SqlUserDbContext>(null);
        }      
        //public DbSet<Customer> Customers { get; set; }     
      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new CustomerConfiguration());          
          
            base.OnModelCreating(modelBuilder);
        }

        public virtual ObjectResult<sp_usermodulModel> sp_usermodulSet(string searchText = "")
        {
            string flag = "01";
            var Params = new object[] { 
                new SqlParameter("@flag", flag),
                new SqlParameter("@search", searchText)
            };
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<sp_usermodulModel>("[myweb_sp_get_table_grid] @flag, @search", Params);
        }

        public virtual ObjectResult<sp_usermasterModel> sp_usermasterSet(string searchText = "")
        {
            string flag = "02";
            var Params = new object[] { 
                new SqlParameter("@flag", flag),
                new SqlParameter("@search", searchText)
            };
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<sp_usermasterModel>("[myweb_sp_get_table_grid] @flag, @search", Params);
        }

        public virtual ObjectResult<sp_officemasterModel> sp_officemasterSet(string searchText = "")
        {
            string flag = "03";
            var Params = new object[] { 
                new SqlParameter("@flag", flag),
                new SqlParameter("@search", searchText)
            };
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<sp_officemasterModel>("[myweb_sp_get_table_grid] @flag, @search", Params);
        }

        public virtual ObjectResult<sp_userclassificationtypeModel> sp_userclassificationtypeSet(string searchText = "")
        {
            string flag = "04";
            var Params = new object[] { 
                new SqlParameter("@flag", flag),
                new SqlParameter("@search", searchText)
            };
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<sp_userclassificationtypeModel>("[myweb_sp_get_table_grid] @flag, @search", Params);
        }

        public virtual ObjectResult<sp_modulemasterModel> sp_modulemasterSet(string searchText = "")
        {
            string flag = "05";
            var Params = new object[] { 
                new SqlParameter("@flag", flag),
                new SqlParameter("@search", searchText)
            };
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<sp_modulemasterModel>("[myweb_sp_get_table_grid] @flag, @search", Params);
        }

        //o20170928
        public virtual ObjectResult<sp_area_master_Model> sp_area_master_Set(string searchText = "")
        {
            string flag = "08";
            var Params = new object[] { 
                new SqlParameter("@flag", flag),
                new SqlParameter("@search", searchText)
            };
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<sp_area_master_Model>("[myweb_sp_get_table_grid] @flag, @search", Params);
        }

        //o20170927
        public virtual ObjectResult<sp_office_master_Model> sp_office_master_Set(string searchText = "")
        {
            string flag = "09";
            var Params = new object[] { 
                new SqlParameter("@flag", flag),
                new SqlParameter("@search", searchText)
            };
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<sp_office_master_Model>("[myweb_sp_get_table_grid] @flag, @search", Params);
        }

        //o20170928
        public virtual ObjectResult<sp_areaMaster_Model> sp_areaMaster_Set(string searchText = "")
        {
            string flag = "12";
            var Params = new object[] { 
                new SqlParameter("@flag", flag),
                new SqlParameter("@search", searchText)
            };
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<sp_areaMaster_Model>("[myweb_sp_get_table_grid] @flag, @search", Params);
        }

    }
}