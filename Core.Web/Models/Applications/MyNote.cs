using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models.Applications
{
    public class MyNote
    {
        public System.Guid id { get; set; }
        public string id_user { get; set; }
        [StringLength(30, ErrorMessage = "Title maksimal 30 karakter.")]
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> posted_date { get; set; }
        public string note_class { get; set; }
        public bool isDeleted { get; set; }
    }
}