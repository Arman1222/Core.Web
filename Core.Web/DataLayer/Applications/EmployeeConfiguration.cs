using MyWeb.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer.Applications
{
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            // Primary Key
            HasKey(p => p.NIK);

            ToTable("MasterEmployee");
            Property(p => p.NIK).HasColumnName("NIK");
            Property(p => p.Group).HasColumnName("Group");
            Property(p => p.Jabatan).HasColumnName("Jabatan");
            Property(p => p.Location).HasColumnName("Location");
            Property(p => p.Nama).HasColumnName("Nama");
            Property(p => p.Cabang).HasColumnName("Cabang");
            Property(p => p.Department).HasColumnName("Department");
            Property(p => p.Director).HasColumnName("Director");
            Property(p => p.Division).HasColumnName("Division");
            Property(p => p.Sts_Data).HasColumnName("Sts_Data");
            Property(p => p.Photo).HasColumnName("Photo");
        
        }
    }
}