using AutoMapper;
using Core.Web.Models.Applications;
using Heroic.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModels.Applications
{
    public class ApplicationViewModel : IMapFrom<Application>, IHaveCustomMappings
	{
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "Application Name harus diisi!")]
        [StringLength(100, ErrorMessage = "Application Nama maksimal 100 karakter.")]
        [Display(Name = "Application Name", Prompt = "Application Name ...")]
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationLink { get; set; }
        public string ApplicationIcon { get; set; }
        public string CreateBy { get; set; }      
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; } 

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationViewModel, Application>()
                .ForMember(d => d.ApplicationId, opt => opt.MapFrom(s => s.ApplicationId))
                .ForMember(d => d.ApplicationName, opt => opt.MapFrom(s => s.ApplicationName))
                .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

            configuration.CreateMap<Application, ApplicationViewModel>()
                .AfterMap((ent, dto) =>
                {
                    dto.CreateBy = ent.UserManager.GetUserName(ent.CreateBy);
                    if (!string.IsNullOrEmpty(ent.UpdateBy))
                        dto.UpdateBy = ent.UserManager.GetUserName(ent.UpdateBy);
                });        
        }        
	}
}

