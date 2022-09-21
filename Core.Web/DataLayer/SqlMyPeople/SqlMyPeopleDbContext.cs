using Core.Web.Models.Applications;
using MyWeb.DataLayer.Applications;
using MyWeb.Models.Applications;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace MyWeb.DataLayer.SqlMyPeople
{
    public class SqlMyPeopleDbContext : DbContext
    {
        static SqlMyPeopleDbContext()
        {
            Database.SetInitializer<SqlMyPeopleDbContext>(null);
        }

        public SqlMyPeopleDbContext()
            : base("SqlMyPeopleConnection")
        {

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Jabatan> JabatanSet { get; set; }
        public DbSet<Division> Divisions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new JabatanConfiguration());
            modelBuilder.Configurations.Add(new EmployeeConfiguration());
            modelBuilder.Configurations.Add(new DivisionConfiguration());
        }

        public virtual ObjectResult<my_people_dashboard_Result> MyPeople_GetDashBoardData(string Nik)
        {
            var nikParameter = Nik != null ?
                new SqlParameter("Nik", Nik) :
                new SqlParameter("Nik", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<my_people_dashboard_Result>("[MyPeople_GetDashBoardData] @Nik", nikParameter);
        }
    }
}
