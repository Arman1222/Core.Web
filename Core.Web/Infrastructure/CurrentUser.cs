using Core.Web.Models.Applications;
using Microsoft.AspNet.Identity;
using MyWeb.DataLayer;
using MyWeb.DataLayer.SqlMyPeople;
using MyWeb.Models;
using MyWeb.Models.Applications;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace MyWeb.Infrastructure.Applications
{
	public class CurrentUser : ICurrentUser
	{
		private readonly IIdentity _identity;
		private readonly ApplicationDbContext _context;

		private ApplicationUser _user;

        private List<string> _roles = new List<string>();
        private List<string> _roleNames = new List<string>();
        private Employee _employee;
        private Department _department;
        private Division _division;
        private Area _area;
        private Branch _branch;
        private Jabatan _jabatan;
       
		public CurrentUser(IIdentity identity, ApplicationDbContext context)
		{
			_identity = identity;
			//_context = context;

            using (_context = new ApplicationDbContext())
            {
                if (identity.AuthenticationType == "Forms") //Forms Authentication
                {
                    _user = _context.Users.Find(_identity.Name);
                }
                else
                {
                    _user = _context.Users.Find(_identity.GetUserId());
                }
                if (_user != null)
                {
                    foreach (var item in _user.Roles)
                    {
                        _roles.Add(item.RoleId);
                        var role = _context.Roles.Find(item.RoleId);
                        if (role != null)
                        {
                            _roleNames.Add(role.Name);
                        }
                    }
                    //Find Branch
                    _branch = _context.BranchSet.Find(_user.BranchId);
                    //Find Area
                    _area = _context.AreaSet.FirstOrDefault(c => c.AreaId == _user.BranchId);                   
                    //Find Department
                    using (var myPeopleCtx = new SqlMyPeopleDbContext())
                    {
                        _employee = myPeopleCtx.Employee.FirstOrDefault(c => c.NIK == _user.NIK);
                        if (_employee != null)
                        {
                            _department = myPeopleCtx.Departments.FirstOrDefault(c => c.Id == _employee.Department);
                            _division = myPeopleCtx.Divisions.FirstOrDefault(c => c.Id == _employee.Division);
                            //Find Jabatan
                            _jabatan = myPeopleCtx.JabatanSet.FirstOrDefault(c => c.Id == _employee.Jabatan);
                        }                        
                    }
                }
            }
		}

		public ApplicationUser User
		{
			get { return _user; }
		}

        public string Username
        {
            get { return _user.UserName; }
        }

        public string[] Roles
        {
            get { return _roles.ToArray(); }
        }
        public string[] RoleNames
        {
            get { return _roleNames.ToArray(); }
        }
        public Employee Employee
        {
            get { return _employee; }
        }
        public Department Department
        {
            get { return _department; }
        }
        public Division Division
        {
            get { return _division; }
        }
        public Area Area
        {
            get { return _area; }
        }
        public Branch Branch
        {
            get { return _branch; }
        }
        public Jabatan Jabatan
        {
            get { return _jabatan; }
        }
	}
}