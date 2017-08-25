using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Repositories
{
    public static class RolePermissionsRepository
    {
        public static IEnumerable<PermissionsListViewModels> GetAllMenuPermissionsByRoleId(this IRepositoryAsync<RolePermissions> repository, int roleId)
        {
            IEnumerable<PermissionsListViewModels> result = new List<PermissionsListViewModels>();
            var list = repository.QueryableCustom().Where(w=>w.IsActive).Select(s=> new {
                menu=s.PagePermission.MenuItem,

            }).ToList();

            List<PermissionsListViewModels> groups = new List<PermissionsListViewModels>();

            //foreach (var item in list.Where(r => r.IsParent && r.ParentId == null).GroupBy(p => p.MenuItemId).Select(g => g.First()).ToList())
            //{
            //    MenuPermissionsListViewModels obj = new MenuPermissionsListViewModels();
            //    obj.GroupName = item.ResourceName;
            //    obj.GroupId = item.ResourceId;
            //    var resourceList = list.Where(w => w.ResourceGroupId == item.ResourceId);
            //    obj.ResourcesList = new List<ResourcePermissionListViewModels>();
            //    foreach (var resource in resourceList.GroupBy(p => p.ResourceId).Select(g => g.First()).ToList())
            //    {
            //        ResourcePermissionListViewModels resourcePermission = new ResourcePermissionListViewModels();
            //        resourcePermission.ResourceName = resource.ResourceName;
            //        resourcePermission.PermissionsList = list.Where(w => w.ResourceId == resource.ResourceId).ToList();
            //        obj.ResourcesList.Add(resourcePermission);
            //    }
            //    groups.Add(obj);
            //}


            return groups;
        }
    }
}