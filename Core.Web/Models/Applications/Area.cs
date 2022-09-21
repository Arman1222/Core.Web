using MyWeb.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Web.Models.Applications
{
    public class Area
    { 
        public int AreaId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }       
        public string CreateBy { get; set; }
        [ForeignKey("CreateBy")]
        public ApplicationUser CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        [ForeignKey("UpdateBy")]
        public ApplicationUser UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Area()
        {
            CreateDate = DateTime.Today;
        }
    }
}