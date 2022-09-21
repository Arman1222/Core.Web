using AutoMapper;
using Heroic.AutoMapper;
using MyWeb.Models;
using System;

namespace MyWeb.ViewModels.Applications
{
    public class ApplicationUserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }       
        public string UserName { get; set; }
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool? Active { get; set; }
        public int? PassMaxWrong { get; set; }
        public int AccessFailedCount { get; set; }  
        public DateTime? LastLogin { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUserViewModel, ApplicationUser>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.UserName))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.LastName))
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName))
                .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

            configuration.CreateMap<ApplicationUser, ApplicationUserViewModel>()
                .ForMember(d => d.Active, opt => opt.MapFrom(s => s.ActiveBit))
                .AfterMap((ent, dto) =>
                {
                   
                });
        }        
    }

    public class UserRoleViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool? Active { get; set; }
        public int? PassMaxWrong { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? LastLogin { get; set; }
        public string RoleHeGet { get; set; }
    }
}