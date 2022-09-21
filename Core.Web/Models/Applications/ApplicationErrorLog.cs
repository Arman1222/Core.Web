using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWeb.Models
{    
    [Table("ApplicationErrorLog")]
    public class ApplicationErrorLog
    {
        //Id	uniqueidentifier
        //ApplicationId	int
        //DateLog	datetime
        //Message	varchar
        //InnerMessage	varchar
        //Tracert	varchar
        //UserId	nvarchar
        public Guid id { get; set; }
        public int ApplicationId { get; set; }
        public DateTime DateLog { get; set; }
        public string Message { get; set; }
        public string InnerMessage { get; set; }
        public string Tracert { get; set; }
        public string UserId { get; set; }
    }
}