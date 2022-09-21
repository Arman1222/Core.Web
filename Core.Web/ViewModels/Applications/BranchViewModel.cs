using Core.Web.Models.Applications;
using Heroic.AutoMapper;

namespace MyWeb.ViewModels.Applications
{
    public class BranchViewModel : IMapFrom<Branch>
    {
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchCodeT24 { get; set; }
        public string BranchName { get; set; }
        public string BranchNameT24 { get; set; } 
        public int AreaId { get; set; }      
        public string AreaName { get; set; }        
    }
}