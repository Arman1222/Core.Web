using AutoMapper;
using Core.Web.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MyWeb;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Core.Web.Models.Applications
{
    public class EntityBase
    {        
        public string GetCurrentUserId()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User.Identity.AuthenticationType == "Forms")
                {
                    return HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    return HttpContext.Current.User.Identity.GetUserId();
                }
            }
            return string.Empty;
        }      
        public void SetUpdateByCurrentUser()
        {
            UpdateBy = GetCurrentUserId();
            UpdateDate = DateTime.Now;
        }        
        public void SetUpdateBy(string userId)
        {
            UpdateBy = userId;
            UpdateDate = DateTime.Now;
        }        
        public void SetApproveByCurrentUser()
        {
            ApproveBy = GetCurrentUserId();
            ApproveDate = DateTime.Now;
        }       
        public void SetApproveBy(string userId)
        {
            ApproveBy = userId;
            ApproveDate = DateTime.Now;
        }       
        public void SetRejectByCurrentUser(string reason)
        {
            RejectReason = reason;
            RejectBy = GetCurrentUserId();
            RejectDate = DateTime.Now;
        }        
        public void SetRejectBy(string userId, string reason)
        {          
            RejectReason = reason;
            RejectBy = userId;
            RejectDate = DateTime.Now;
        }

        private ApplicationUserManager _userManager;
        [NotMapped]
        [JsonIgnore]
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        [NotMapped]
        [JsonIgnore]
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public EntityBase()
        {
            //_createDate = DateTime.Now;           
            
        }

        private string _createBy;

        [IgnoreMap]
        public string CreateBy { 
            get { 
                return _createBy; 
            } 
            set 
            {               
                _createBy = value;        
            } 
        }
        //[ForeignKey("CreateBy")]
        //public virtual ApplicationUser CreateUser { get; set; }

        private DateTime? _createDate;
       
        public DateTime? CreateDate { 
            get { 
                return _createDate; 
            } 
            set {
                if (value == null)
                {
                    _createDate = DateTime.Now;
                    string userId = GetCurrentUserId();
                    if (!string.IsNullOrEmpty(userId))
                    {
                        _createBy = userId;
                        BranchId = UserManager.GetBranch(userId);
                        CompanyId = UserManager.GetCompany(userId);
                    }
                }
                else
                {
                    _createDate = value;
                }                         
            } 
        }       

        [IgnoreMap]
        public string UpdateBy { get; set; }
        //[ForeignKey("UpdateBy")]
        //public ApplicationUser UpdateUser { get; set; }
        //private DateTime? _updateDate;

        private DateTime? _updateDate;

        [IgnoreMap]
        public DateTime? UpdateDate
        {
            get
            {
                return _updateDate;
            }
            set
            {
                _updateDate = value;
                string userId = GetCurrentUserId();
                if (!string.IsNullOrEmpty(userId))
                {                    
                    BranchId = UserManager.GetBranch(userId);
                    CompanyId = UserManager.GetCompany(userId);
                }
            }
        }       
        //public DateTime? UpdateDate { get; set; }
        //public DateTime? UpdateDate { 
        //    get { 
        //        return _updateDate; 
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _updateDate = DateTime.Now;

        //            if (HttpContext.Current != null)
        //            {
        //                if (HttpContext.Current.User.Identity.AuthenticationType == "Forms")
        //                {
        //                    //Forms Authentication
        //                    UpdateBy = HttpContext.Current.User.Identity.Name;
        //                    //Forms Authentication//
        //                }
        //                else
        //                {
        //                    UpdateBy = HttpContext.Current.User.Identity.GetUserId();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            _updateDate = value;
        //        } 
        //    }
        //}
        [IgnoreMap]
        public string ApproveBy { get; set; }
        //[ForeignKey("UpdateBy")]
        //public ApplicationUser UpdateUser { get; set; }
        public DateTime? ApproveDate { get; set; }
        [IgnoreMap]
        public string RejectBy { get; set; }       
        public DateTime? RejectDate { get; set; }
        public string RejectReason { get; set; }   
        public int? CompanyId { get; set; }
        //[ForeignKey("CompanyId")]
        //public Company Company { get; set; }
        public int? BranchId { get; set; }
        //[ForeignKey("BranchId")]
        //public virtual Branch Branch { get; set; }
        public StatusData StatusData { get; set; }
        public bool IsDelete { get; set; }
    }
}
