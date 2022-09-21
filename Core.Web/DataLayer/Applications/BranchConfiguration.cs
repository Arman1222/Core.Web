using Core.Web.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer
{
    public class BranchConfiguration : EntityTypeConfiguration<Branch>
    {
        public BranchConfiguration()
        {
            // Primary Key
            HasKey(p => p.BranchId);

            Property(c => c.BranchName)
            .HasMaxLength(100)
            .IsRequired();         

            ToTable("OfficeMaster");          
            Property(p => p.BranchId).HasColumnName("Id");
            Property(p => p.BranchCode).HasColumnName("OfficeId");
            Property(p => p.BranchCodeT24).HasColumnName("OfficeT24_KD_Cabang");
            Property(p => p.BranchName).HasColumnName("OfficeDesc");
            Property(p => p.BranchNameT24).HasColumnName("OfficeT24_Nama_Cabang");
          

        }
    }
}