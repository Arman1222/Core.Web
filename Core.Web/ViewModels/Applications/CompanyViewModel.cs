using AutoMapper;
using Core.Web.Models.Applications;
using Heroic.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModels.Applications
{
	public class CompanyViewModel : IMapFrom<Company>,IHaveCustomMappings
	{
        public int CompanyId { get; set; }      
		[Required(ErrorMessage="Company Name harus diisi!")]
        [StringLength(50, ErrorMessage = "Company Name maksimal 50 karakter.")]
        [Display(Name = "Company Name", Prompt = "...")]
		public string CompanyName { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
		
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<CompanyViewModel, Company>()
                .ForMember(d => d.CompanyId, opt => opt.MapFrom(s => s.CompanyId))
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.CompanyName))               
                .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

            configuration.CreateMap<Company, CompanyViewModel>()               
                .AfterMap((ent, dto) => {                    
                        dto.CreateBy = ent.UserManager.GetUser(ent.CreateBy).UserName;
                        if(!string.IsNullOrEmpty(ent.UpdateBy))
                            dto.UpdateBy = ent.UserManager.GetUser(ent.UpdateBy).UserName;
               });         
        }
	}
}

