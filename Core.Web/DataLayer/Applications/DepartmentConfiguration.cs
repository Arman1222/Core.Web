using MyWeb.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer.Applications
{
    public class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            // Primary Key
            HasKey(p => p.Id);

            ToTable("MasterDepartment");          
            Property(p => p.Id).HasColumnName("Dept_Id");
            Property(p => p.Name).HasColumnName("Dept_Desc");            

        }
    }
}