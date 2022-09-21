using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModels.Menus
{
    public class NavbarViewModel
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }       

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must enter a name.")]
        [StringLength(50, ErrorMessage = "The menu name must be 50 characters or shorter.")]
        [Display(Name = "Menu Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must enter menu text.")]
        [StringLength(50, ErrorMessage = "The menu text must be 50 characters or shorter.")]
        [Display(Name = "Menu Text")]
        public string Text { get; set; }
     
        [StringLength(30, ErrorMessage = "The Controller must be 30 characters or shorter.")]
        [Display(Name = "Controller")]
        public string Controller { get; set; }
     
        [StringLength(30, ErrorMessage = "The Controller must be 30 characters or shorter.")]
        [Display(Name = "Action")]
        public string Action { get; set; }

        [Required(ErrorMessage = "You must choose Image Class.")]
        [Display(Name = "Image Class")]
        public int? ImageClassId { get; set; }

        [Required(ErrorMessage = "You must choose Application.")]
        [Display(Name = "Application")]
        public int? ApplicationId { get; set; }
        public int? SortId { get; set; }
    
    }
}