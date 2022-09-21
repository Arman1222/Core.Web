using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModels.euser
{
    public class usermasterModel
    {
        [Required, Display(Name = "Username"), MaxLength(40)]
        public string Name { get; set; }
        [Required, Display(Name = "User Real Name"), MaxLength(40)]
        public string RealName { get; set; }
        [Display(Name = "Active")]
        public Active ActiveBit { get; set; }
        [MaxLength(40)]
        public string password { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int MaxPassWrong { get; set; }
        [MaxLength(5)]
        public string OfficeId { get; set; }
        [Display(Name = "N I K"), MaxLength(40)]
        public string NIK { get; set; }
    }

    public enum Active
    {
        NO = 0,
        YES = 1
    }

}