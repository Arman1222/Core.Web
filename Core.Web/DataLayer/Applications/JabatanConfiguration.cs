using MyWeb.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer.Applications
{
    public class JabatanConfiguration : EntityTypeConfiguration<Jabatan>
    {
        public JabatanConfiguration()
        {
            // Primary Key
            HasKey(p => p.Id);

            ToTable("MasterJabatan");
            Property(p => p.Id).HasColumnName("JabID");
            Property(p => p.Name).HasColumnName("JabDesc");            

        }
    }
}