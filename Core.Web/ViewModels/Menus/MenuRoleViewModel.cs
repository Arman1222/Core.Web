using MyWeb.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModels.Menus
{
    public class MenuRoleViewModel 
    {
        [Required]
        public string RoleId { get; set; }

        [Required]
        public int MenuId { get; set; }
        public string ApplicationName { get; set; }
        public List<Navbar> Menus { get; set; }
        public List<ApplicationRole> Roles { get; set; }       
       
    }
}