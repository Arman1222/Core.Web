using System.ComponentModel.DataAnnotations;
using Heroic.AutoMapper;
using MyWeb.Models.Customers;
using System;
using AutoMapper;


namespace MyWeb.ViewModels.Customers
{
	public class CustomerViewModel : IMapFrom<Customer>,IHaveCustomMappings
	{
        public int CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }

		[Required(ErrorMessage="Nama harus diisi!")]
        [StringLength(50, ErrorMessage = "Nama maksimal 50 karakter.")]
        [Display(Name = "Name", Prompt = "Name (ex: John Doe)...")]
		public string Name { get; set; }

		[Required, DataType(DataType.EmailAddress)]
		public string Email { get; set; }
	
		[Required, DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }	

		[Required, DataType(DataType.MultilineText)]
		public string Address { get; set; }
	    
        [Required]
        public DateTime? TerminationDate { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Required]
        public int ImageClassId { get; set; }
        
        [Range(typeof(int), "0", "10000000")]       
        public int Total { get; set; }
        [Range(typeof(decimal), "0", "10000")]
        public decimal Nominal { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; } 

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<CustomerViewModel, Customer>()
                .ForMember(d => d.CustomerId, opt => opt.MapFrom(s => s.CustomerId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address))
                .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Phone))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
                .ForMember(d => d.TerminationDate, opt => opt.MapFrom(s => s.TerminationDate))
                .ForMember(d => d.StartTime, opt => opt.MapFrom(s => s.StartTime))
                .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull)); 

       
            
            //http://stackoverflow.com/questions/18234616/doing-arithmetic-in-automapper-and-entity-framework
            configuration.CreateMap<Customer, CustomerViewModel>()
                //.ForMember(dto => dto.CreateBy, m => m.Ignore())
                //.ForMember(dto => dto.UpdateBy, m => m.Ignore())
                .AfterMap((ent, dto) => {                    
                        dto.CreateBy = ent.UserManager.GetUser(ent.CreateBy).UserName;
                        if(!string.IsNullOrEmpty(ent.UpdateBy))
                            dto.UpdateBy = ent.UserManager.GetUser(ent.UpdateBy).UserName;
               });         
        }
	}
}

