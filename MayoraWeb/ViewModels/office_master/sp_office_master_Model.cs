using System;
using System.ComponentModel.DataAnnotations;
//o20170927
namespace MyWeb.ViewModels.office_master
{
    public class sp_office_master_Model
    {
        [Required(ErrorMessage = "Office Id Harus Diisi!!")]
        [StringLength(3, ErrorMessage = "Maksimal 3 karakter")]
        //[Display(Prompt = "Must Unique")]
        //[Required]
        public string OfficeId { get; set; }


        [Required(ErrorMessage = "Office Desc Harus Diisi!!")]
        [StringLength(40, ErrorMessage = "Maksimal 40 karakter")]
        //[Required]
        public string OfficeDesc { get; set; }


        [Required(ErrorMessage = "Branch Code T24 Harus Diisi!!")]
        [StringLength(9, ErrorMessage = "Maksimal 9 karakter")]
        [Display(Name = "Branch Code T24")]
        //[Required]
        //[Display(Prompt = "Must Unique")]
        public string OfficeT24_KD_Cabang { get; set; }


        [Required(ErrorMessage = "Branch Name T24 Harus Diisi!!")]
        [StringLength(40, ErrorMessage = "Maksimal 40 karakter")]
        [Display(Name = "Branch Name T24")]
        //[Required]
        public string OfficeT24_Nama_Cabang { get; set; }


        public int? Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }

    }
}