using MyWeb.DataLayer.SqlMyPeople;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MyWeb.DataLayer.Applications
{
    public class ApplicationDataHelper
    {
        private ApplicationDbContext _appDbContext = new ApplicationDbContext();
        private SqlMyPeopleDbContext _sqlMyPeopleDbContext = new SqlMyPeopleDbContext();

        public async Task<string> GetBranchName(string branchCode)
        {
            var model = await _appDbContext.BranchSet.FirstOrDefaultAsync(c => c.BranchCode == branchCode);
            if (model != null)
            {
                return model.BranchName;
            }
            return null;
        }

        public async Task<string> GetDepartmentName(string id)
        {
            var model = await _sqlMyPeopleDbContext.Departments.FirstOrDefaultAsync(c => c.Id == id);
            if (model != null)
            {
                return model.Name;
            }
            return null;
        }

    }
}