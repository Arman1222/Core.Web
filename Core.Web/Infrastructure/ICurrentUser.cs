using Core.Web.Models.Applications;
using MyWeb.Models;
using MyWeb.Models.Applications;

namespace MyWeb.Infrastructure.Applications
{
    public interface ICurrentUser
    {
        ApplicationUser User { get; }
        string Username { get; }
        string[] Roles { get; }
        string[] RoleNames { get; }
        Employee Employee { get; }
        Department Department { get; }
        Division Division { get; }
        Area Area { get; }
        Branch Branch { get; }
        Jabatan Jabatan { get; }
    }
}
