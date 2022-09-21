using Core.Web.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer
{
    public class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            Property(c => c.CompanyName)
            .HasMaxLength(100)
            .IsRequired();

            Ignore(c => c.RoleManager);

            ToTable("CompanyMaster");          
        }
    }
}