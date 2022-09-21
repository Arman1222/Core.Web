using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Web.Models.Applications
{
    public class Application : EntityBase
    {
        public int ApplicationId { get; set; }
        //http://stackoverflow.com/questions/21573550/setting-unique-constraint-with-fluent-api
        //http://stackoverflow.com/questions/18889218/unique-key-constraints-for-multiple-columns-in-entity-framework
        [Index("Application_ApplicationName", IsUnique = true)]
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationLink { get; set; }
        public string ApplicationIcon { get; set; }
        //public string CreateBy { get; set; }
        //[ForeignKey("CreateBy")]
        //public ApplicationUser CreateUser { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string UpdateBy { get; set; }
        //[ForeignKey("UpdateBy")]
        //public ApplicationUser UpdateUser { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public Application()
        //{
        //    CreateDate = DateTime.Today;
        //}
    }
}