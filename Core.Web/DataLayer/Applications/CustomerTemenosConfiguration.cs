using MyWeb.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer.Applications
{
    public class CustomerTemenosConfiguration : EntityTypeConfiguration<CustomerTemenos>
    {
        public CustomerTemenosConfiguration()
        {
            // Primary Key
            HasKey(p => p.CifNo);

            ToTable("Customer");          
            //Property(p => p.Id).HasColumnName("@ID");
            Property(p => p.CifNo).HasColumnName("customer_no");
            Property(p => p.Mnemonic).HasColumnName("mnemonic");
            Property(p => p.Nama).HasColumnName("name_1");
            Property(p => p.Pob).HasColumnName("place_of_birth");
            Property(p => p.Dob).HasColumnName("date_of_birth");
            Property(p => p.Alamat).HasColumnName("street");
            Property(p => p.Kelurahan).HasColumnName("ktp_kelurahan");
            Property(p => p.Kecamatan).HasColumnName("ktp_kecamatan");
            Property(p => p.PostCode).HasColumnName("post_code");
            Property(p => p.Gender).HasColumnName("gender");

        }
    }
}