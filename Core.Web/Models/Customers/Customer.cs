using Core.Web.Models.Applications;
using MyWeb.Models;
using MyWeb.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Models.Customers
{
    public class Customer : EntityBase
    {
        public int CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }      
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }       
        public DateTime? TerminationDate { get; set; }
        public DateTime StartTime { get; set; }
        //public ImageClass ImageClass { get; set; }
        public int ImageClassId { get; set; }
        public int Total { get; set; }
        public decimal Nominal { get; set; } 
        public Customer() : base()
        {           
            StartTime = DateTime.Today;
        }
    }
}