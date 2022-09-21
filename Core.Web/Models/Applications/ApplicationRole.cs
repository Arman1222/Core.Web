using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace MyWeb.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }
        public ApplicationRole(string name) : base(name)
        {
        }
        public virtual ICollection<Navbar> Menus { get; set; }
    }
}