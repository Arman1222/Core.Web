using Core.Web.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer
{
    public class AreaConfiguration : EntityTypeConfiguration<Area>
    {
        public AreaConfiguration()
        {
            // Primary Key
            HasKey(p => p.AreaId);

            Property(c => c.AreaCode)
            .HasMaxLength(3)
            .IsRequired();

            Property(c => c.AreaName)
            .HasMaxLength(100)
            .IsRequired();

            ToTable("AreaMaster");                             

        }
    }
}