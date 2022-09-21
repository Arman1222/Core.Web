using MyWeb.DataLayer.Customers;
using MyWeb.Models.Customers;
using System.Data.Entity;

namespace MyWeb.DataLayer.SqlCore
{
    public class SqlCoreDbContext : DbContext
    {
        public SqlCoreDbContext()
            : base("SqlCoreConnection")
        {
            Database.SetInitializer<SqlCoreDbContext>(null);
        }      
        public DbSet<Customer> Customers { get; set; }     
      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerConfiguration());          
          
            base.OnModelCreating(modelBuilder);
         
        }

    }
}
