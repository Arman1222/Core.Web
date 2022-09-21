using System;

namespace Core.Web.Models.Applications
{
    public class MyTask
    {
        public System.Guid id { get; set; }
        public string id_user { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<bool> fixdate { get; set; }
        public Nullable<System.DateTime> startdate { get; set; }
        public Nullable<System.DateTime> enddate { get; set; }
        public string posted_by { get; set; }
        public Nullable<System.DateTime> posted_date { get; set; }
        public Nullable<System.DateTime> finishdate { get; set; }
        public string note { get; set; }
        public Nullable<int> progres { get; set; }
        public Nullable<bool> alert_me { get; set; }
        public bool isDeleted { get; set; }
    }
}