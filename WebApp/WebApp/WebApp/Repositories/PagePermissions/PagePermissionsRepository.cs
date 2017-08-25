using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Repositories
{
    public static class PagePermissionsRepository
    {
        public static IEnumerable<PermissionsListViewModels> GetAllMenuPermissionsByRoleId(this IRepositoryAsync<PagePermissions> repository, int roleId)
        {
            List<PermissionsListViewModels> groupList = new List<PermissionsListViewModels>();
            //best query for menu render
            groupList = repository.QueryableCustom().Where(w => w.IsActive && (w.MenuItem.GroupId==0 || w.MenuItem.GroupId==null)).Select(s => new PermissionsListViewModels
            {
                GroupId = s.MenuItem.MenuItemId,
                MenuGroup = new MenuViewModels { MenuItem = s.MenuItem, PagePermissions = s.MenuItem.PagePermissions.Select(gp => new PermissionsViewModels { CanAccess = gp.RolePermissions.Where(gw => gw.RoleId == roleId && gw.IsActive).Count() > 0, PermissionId = gp.PermissionId, PermissionText = gp.PermissionText }) },
                MenuItemsList = s.MenuItem.MenuItemsList.Select(m => new MenuViewModels { MenuItem = m, PagePermissions = m.PagePermissions.Select(mm => new PermissionsViewModels { CanAccess = mm.RolePermissions.Where(mw => mw.RoleId == roleId && mw.IsActive).Count() > 0, PermissionId = mm.PermissionId, PermissionText = mm.PermissionText }) })
            }).GroupBy(p => p.GroupId).Select(g => g.FirstOrDefault()).ToList();

            
            return groupList;
        }

        public static IEnumerable<PagePermissions> GetAllPermissionsByResourceId(this IRepositoryAsync<PagePermissions> repository, int resourceId)
        {
            return repository.QueryableCustom().Where(w => w.MenuItemId == resourceId && w.IsActive);
        }

        private static void ProcessList()
        {

        }
    }
}