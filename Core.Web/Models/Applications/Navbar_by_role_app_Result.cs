//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Web.Models.Applications
{
    using System;
    
    public partial class Sp_get_navbar_by_role_app_Result
    {
        public System.Guid MenuId { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public string faicon { get; set; }
        public string description { get; set; }
        public Nullable<int> ImageClassId { get; set; }
        public string Activeli { get; set; }
        public bool Status { get; set; }
        public bool IsParent { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string ApproveBy { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public bool IsDelete { get; set; }
        public string RejectBy { get; set; }
        public Nullable<System.DateTime> RejectDate { get; set; }
        public string RejectReason { get; set; }
        public int StatusData { get; set; }
        public Nullable<int> SortId { get; set; }
        public bool Used { get; set; }
    }
}
