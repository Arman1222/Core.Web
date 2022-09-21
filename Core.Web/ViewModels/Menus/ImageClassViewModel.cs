using Heroic.AutoMapper;
using MyWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyWeb.ViewModels.Menus
{
    public class ImageClassViewModel : IMapFrom<ImageClass>
    {
        public int ImageClassId { get; set; }
      
        [Required(ErrorMessage = "You must enter a name.")]
        [StringLength(50, ErrorMessage = "Name names must be 50 characters or shorter.")]
        [AdditionalMetadata("maxLength", 30)]
        [Display(Name = "Image Class Name", Prompt = "Imaga Class Name ...")]
        public string ImageClassName { get; set; }
       
    }
}