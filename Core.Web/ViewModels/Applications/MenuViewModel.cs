using AutoMapper;
using Heroic.AutoMapper;
using MyWeb.Models;

namespace MyWeb.ViewModels.Applications
{
    public class MenuViewModel : IMapFrom<Navbar>,IHaveCustomMappings
	{
		public string Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }
    
		public void CreateMappings(IConfiguration configuration)
		{
            configuration.CreateMap<Navbar, MenuViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Text, opt => opt.MapFrom(s => s.Text));
		}
	}
}