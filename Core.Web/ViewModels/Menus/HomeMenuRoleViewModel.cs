using AutoMapper;
using Heroic.AutoMapper;
using MyWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModels.Menus
{
    public class HomeMenuRoleViewModel : IMapFrom<HomeMenuRole>, IHaveCustomMappings
    {
        public int id { get; set; }
        public string RoleId { get; set; }
        public string Application { get; set; }
        public string RoleName { get; set; }
        [MaxLength(50)]
        public string FolderView { get; set; }
        [MaxLength(50)]
        public string NameCshtml { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public int? ApilcationId { get; set; }
        public bool IsDelete { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<HomeMenuRoleViewModel, HomeMenuRole>();

            configuration.CreateMap<HomeMenuRole, HomeMenuRoleViewModel>()
                .ForMember(d => d.RoleName, opt => opt.MapFrom(s => s.Role.Name))
                .ForMember(d => d.Application, opt => opt.MapFrom(s => s.Application.ApplicationName))
                .AfterMap((ent, dto) =>
                {
                   
                });
        }
    }
}