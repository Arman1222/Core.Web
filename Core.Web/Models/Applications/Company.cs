using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Web.Models.Applications
{
    public class Company : EntityBase
    {        
        public int CompanyId { get; set; }
        [Index("Company_CompanyName", IsUnique = true)]
        public string CompanyName { get; set; }
        //public string CreateBy { get; set; }
        //[ForeignKey("CreateBy")]
        //public ApplicationUser CreateUser { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string UpdateBy { get; set; }
        //[ForeignKey("UpdateBy")]
        //public ApplicationUser UpdateUser { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public Company()
        //{
        //    CreateDate = DateTime.Today;
        //}
    }
}