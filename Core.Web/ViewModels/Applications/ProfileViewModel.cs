using AutoMapper;
using Heroic.AutoMapper;
using MyWeb.Models;
using System;

namespace MyWeb.ViewModels.Applications
{
    public class ProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
	{
		public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }      
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
		public string Email { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool ActiveBit { get; set; }
        public int AccessFailedCount { get; set; }
        public int PassMaxWrong { get; set; }
        public DateTime LastLogin { get; set; }

		public void CreateMappings(IConfiguration configuration)
		{
            configuration.CreateMap<ApplicationUser, ProfileViewModel>()
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.LastName))
                //.ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address))
                //.ForMember(d => d.City, opt => opt.MapFrom(s => s.City))
                //.ForMember(d => d.State, opt => opt.MapFrom(s => s.State))
                //.ForMember(d => d.ZipCode, opt => opt.MapFrom(s => s.ZipCode))
				.ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email));
		}
	}
}