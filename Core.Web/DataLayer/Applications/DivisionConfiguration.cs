using MyWeb.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer.Applications
{
    public class DivisionConfiguration : EntityTypeConfiguration<Division>
    {
        public DivisionConfiguration()
        {
            // Primary Key
            HasKey(p => p.Id);

            ToTable("MasterDivision");
            Property(p => p.Id).HasColumnName("Div_ID");
            Property(p => p.Name).HasColumnName("Div_Desc");            

        }
    }
}