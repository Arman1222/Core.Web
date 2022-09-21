using Core.Web.Models.Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWeb.Models
{
    public class Navbar : EntityBase
    {
        [Key]
        public Guid Id { get; set; }
        private Guid? _parentId { get; set; }
        public Guid? ParentId
        { 
            get
            {
                return _parentId;
            } 
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("Menu tidak dapat memiliki parent dirinya sendiri");
                _parentId = value;
            }    
        }
        public virtual Navbar Parent { get; set; }
        [ForeignKey("ParentId")]
        public IList<Navbar> Children { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Text { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public string description { get; set; }
        [Required]
        public string faicon { get; set; }
        //public string ImageClass { get; set; }
        public int? ImageClassId { get; set; }
        public virtual ImageClass ImageClass { get; set; }
        public string Activeli { get; set; }
        public bool Status { get; set; }        
        public bool IsParent { get; set; }
        public int? ApplicationId { get; set; }
        public int? SortId { get; set; }
        public virtual Application Application { get; set; }
        public virtual ICollection<ApplicationRole> Roles { get; set; }
    }
}