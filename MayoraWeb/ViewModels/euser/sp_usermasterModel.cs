using System;

namespace MyWeb.ViewModels.euser
{
    public class sp_usermasterModel
    {
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Office { get; set; }
        public string Status { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int? MaxPassWrong { get; set; }
        public string OfficeId { get; set; }
        public DateTime? LastLogin { get; set; }
        public string NIK { get; set; }
        public Active ActiveBit { get; set; }
    }
}