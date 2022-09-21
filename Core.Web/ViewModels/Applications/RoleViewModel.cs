using AutoMapper;
using Heroic.AutoMapper;
using MyWeb.Models;

namespace MyWeb.ViewModels.Applications
{
    public class RoleViewModel : IMapFrom<ApplicationRole>, IHaveCustomMappings
	{
		public string Id { get; set; }

        public string Name { get; set; } 
    
		public void CreateMappings(IConfiguration configuration)
		{
            configuration.CreateMap<ApplicationRole, RoleViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));
		}
	}
}