using MyWeb.DataLayer.Applications;
using MyWeb.Models.Applications;
using System.Data.Entity;

namespace MyWeb.DataLayer.SqlTemenos
{
    public class SqlTemenosDbContext : DbContext
    {
        public SqlTemenosDbContext()
            : base("SqlTemenosConnection")
        {
            Database.SetInitializer<SqlTemenosDbContext>(null);
        }
        public DbSet<CustomerTemenos> CustomerTemenosSet { get; set; }     
      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerTemenosConfiguration());          
          
            base.OnModelCreating(modelBuilder);
         
        }

    }
}
