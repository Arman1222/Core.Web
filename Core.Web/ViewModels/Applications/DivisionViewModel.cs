using Heroic.AutoMapper;
using MyWeb.Models.Applications;

namespace MyWeb.ViewModels.Applications
{
    public class DivisionViewModel : IMapFrom<Division>
    {
        public string Id { get; set; }
        public string Name { get; set; }     
       
    }
}