using Core.Web.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer
{
    public class ApplicationMasterConfiguration : EntityTypeConfiguration<Application>
    {
        public ApplicationMasterConfiguration()
        {
            Property(c => c.ApplicationName)
            .HasMaxLength(100)
            .IsRequired();

            Ignore(c => c.RoleManager);

            ToTable("ApplicationMaster");          
        }
    }
}