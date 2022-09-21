using Heroic.AutoMapper;
using MyWeb.Models.Applications;
using System;

namespace MyWeb.ViewModels.Applications
{
    public class CustomerTemenosViewModel : IMapFrom<CustomerTemenos>
    {
        //public string Id { get; set; }
        public string CifNo { get; set; }
        public string Mnemonic { get; set; }
        public string Nama { get; set; }
        public string Pob { get; set; }
        public DateTime? Dob { get; set; }
        public string Alamat { get; set; }
        public string Kelurahan { get; set; }
        public string Kecamatan { get; set; }
        public string PostCode { get; set; }
        public string Gender { get; set; }          
       
    }
}