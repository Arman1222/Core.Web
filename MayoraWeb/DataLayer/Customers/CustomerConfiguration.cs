using MyWeb.Models.Customers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer.Customers
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            //Property(c => c.AccountNumber)
            //    .HasMaxLength(8)
            //    .IsRequired()
            //    .HasColumnAnnotation("Index",
            //        new IndexAnnotation(new IndexAttribute("AK_Customer_AccountNumber") {IsUnique = true}));

            Property(c => c.Name)
                .HasMaxLength(30)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_Customer_Name") { IsUnique = true }));

            Property(c => c.Address).HasMaxLength(30).IsRequired();
            //Property(c => c.City).HasMaxLength(15).IsRequired();
            //Property(c => c.State).HasMaxLength(2).IsRequired();
            //Property(c => c.ZipCode).HasMaxLength(10).IsRequired();
            Property(c => c.Phone).HasMaxLength(15).IsOptional();
            Property(c => c.Email).HasMaxLength(100).IsRequired();

        }
    }
}