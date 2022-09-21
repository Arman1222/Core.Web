using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModels.Applications
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must enter a name for the Role.")]
        [StringLength(256, ErrorMessage = "The role name must be 256 characters or shorter.")]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}