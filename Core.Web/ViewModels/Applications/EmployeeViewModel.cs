using Heroic.AutoMapper;
using MyWeb.Models.Applications;

namespace MyWeb.ViewModels.Applications
{
    public class EmployeeViewModel : IMapFrom<Employee>
    {
        public string Nik { get; set; }
        public string Nama { get; set; }
        public string Group { get; set; }
        public string Cabang { get; set; }
        public string Location { get; set; }
        public string Division { get; set; }
        public string Jabatan { get; set; }
        public string Department { get; set; }
        public string Director { get; set; }
        public string Sts_Data { get; set; }       
       
    }
}