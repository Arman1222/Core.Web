using Heroic.AutoMapper;
using MyWeb.Models.Applications;

namespace MyWeb.ViewModels.Applications
{
    public class DepartmentViewModel : IMapFrom<Department>
    {
        public string Id { get; set; }
        public string Name { get; set; }     
       
    }
}