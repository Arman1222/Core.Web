using MyWeb.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer
{
    public class MenuConfiguration : EntityTypeConfiguration<Navbar>
    {
        public MenuConfiguration()
        {
            ToTable("WebMenuMaster");
                
            Ignore(c => c.RoleManager);

            Property(c => c.Id).HasColumnName("MenuId");
            HasKey(c => c.Id);
            Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("Web_Menu_Name") {IsUnique = true}));

            Property(c => c.Text)
                .HasMaxLength(50)
                .IsRequired();

            Property(c => c.Controller)
                .HasMaxLength(30)
                .IsOptional();

            Property(c => c.Action)
                .HasMaxLength(30)
                .IsOptional();

            Property(c => c.Area)
                .HasMaxLength(30)
                .IsOptional();

            Property(c => c.Activeli)
               .HasMaxLength(30)
               .IsOptional();

            HasOptional(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .WillCascadeOnDelete(false);

            //HasOptional(c => c.CreateUser)
            //  .WithMany()
            //  .HasForeignKey(c => c.CreateBy)
            //  .WillCascadeOnDelete(false);

            //HasOptional(c => c.UpdateUser)
            //  .WithMany()
            //  .HasForeignKey(c => c.UpdateBy)
            //  .WillCascadeOnDelete(false);
        }
    }

}