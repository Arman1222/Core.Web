using MyWeb.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Web.Models.Applications
{
    public class Branch
    { 
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchCodeT24 { get; set; }
        public string BranchName { get; set; }
        public string BranchNameT24 { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public virtual Area Area { get; set; }
        public string CreateBy { get; set; }
        [ForeignKey("CreateBy")]
        public ApplicationUser CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        [ForeignKey("UpdateBy")]
        public ApplicationUser UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Branch()
        {
            CreateDate = DateTime.Today;
        }
    }
}