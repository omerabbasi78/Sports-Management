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
    public interface IPagePermissionsService : IService<PagePermissions>
    {
        IEnumerable<PermissionsListViewModels> GetAllMenuPermissionsByRoleId(int roleId);
        IEnumerable<PagePermissions> GetAllPermissionsByResourceId(int resourceId);
    }
}
