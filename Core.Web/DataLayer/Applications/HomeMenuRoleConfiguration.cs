using MyWeb.Models;
using System.Data.Entity.ModelConfiguration;

namespace MyWeb.DataLayer
{
    public class HomeMenuRoleConfiguration : EntityTypeConfiguration<HomeMenuRole>
    {
        public HomeMenuRoleConfiguration()
        {
            ToTable("HomeMenuRole");
        }
    }
}