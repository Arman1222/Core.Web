using Core.Web.Models.Applications;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer
{
    public class MyNoteConfiguration : EntityTypeConfiguration<MyNote>
    {
        public MyNoteConfiguration()
        {
            // Primary Key
            HasKey(p => p.id);
            ToTable("MyNote");
        }
    }
    public class MyTaskConfiguration : EntityTypeConfiguration<MyTask>
    {
        public MyTaskConfiguration()
        {
            // Primary Key
            HasKey(p => p.id);
            ToTable("MyTask");
        }
    }
}