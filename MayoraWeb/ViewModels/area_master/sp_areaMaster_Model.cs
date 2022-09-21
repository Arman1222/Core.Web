using System;
using System.ComponentModel.DataAnnotations;
//o20170928
namespace MyWeb.ViewModels.area_master
{
    public class sp_areaMaster_Model
    {
        //[Required(ErrorMessage = "Area Id Harus Diisi!!")]
        //[Display(Prompt = "Must Unique")]
        public int? AreaId { get; set; }


        [Required(ErrorMessage = "Area Code Harus Diisi!!")]
        [StringLength(3, ErrorMessage = "Maksimal 3 karakter")]
        public string AreaCode { get; set; }


        [Required(ErrorMessage = "Area Name Harus Diisi!!")]
        [StringLength(100, ErrorMessage = "Maksimal 100 karakter")]
        public string AreaName { get; set; }
        
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }

    }
}