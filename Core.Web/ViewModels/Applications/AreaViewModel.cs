using Core.Web.Models.Applications;
using Heroic.AutoMapper;

namespace MyWeb.ViewModels.Applications
{
    public class AreaViewModel : IMapFrom<Area>
    {
        public int AreaId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; } 
       
    }
}