using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWeb.Models
{
    [Table("HomeMenuRole")]
    public class HomeMenuRole
    {
        [Key]
        public int id { get; set; }
        public string FolderView { get; set; }
        public string NameCshtml { get; set; }
        public string Title { get; set; }
        public int? ApilcationId { get; set; }
        public string RoleId { get; set; }
        public ApplicationRole Role
        {
            get
            {
                using (var ctx = new MyWeb.DataLayer.ApplicationDbContext())
                {
                    return ctx.Roles.Find(this.RoleId);
                }
            }
        }

        public Core.Web.Models.Applications.Application Application
        {
            get
            {
                using (var ctx = new MyWeb.DataLayer.ApplicationDbContext())
                {
                    return ctx.ApplicationSet.Find(this.ApilcationId);
                }
            }
        }
        public bool IsDelete { get; set; }
    }
}