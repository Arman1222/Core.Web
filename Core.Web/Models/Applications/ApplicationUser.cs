using Core.Web.Models.Applications;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }       
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string ZipCode { get; set; }
        //public List<WorkOrder> WorkOrders { get; set; }

        //private string _fullName;
        public string FullName
        {
            get;
            set; 
            //get
            //{
            //    return _fullName;
            //}
            //set
            //{
            //    if (!string.IsNullOrEmpty(FirstName))
            //    {
            //        _fullName = FirstName + " " + LastName ?? "";
            //    }
            //}
        }


        //public string AddressBlock
        //{
        //    get
        //    {
        //        string addressBlock = string.Format("{0}<br/>{1}, {2} {3}", Address, City, State, ZipCode).Trim();
        //        return addressBlock == "<br/>," ? string.Empty : addressBlock;
        //    }
        //}
        public DateTime? ExpireDate { get; set; }
        public bool? ActiveBit { get; set; }
        public int? PassMaxWrong { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public int? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}