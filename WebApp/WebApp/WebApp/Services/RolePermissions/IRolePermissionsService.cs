using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public interface IRolePermissionsService : IService<RolePermissions>
    {
        IEnumerable<PermissionsListViewModels> GetAllMenuPermissionsByRoleId(int roleId);
    }
}
