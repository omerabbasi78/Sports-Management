using Repository.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.HelperClass;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.ViewModels
{
    public class PermissionsViewModels
    {
        public string PermissionText { get; set; }
        public int PermissionId { get; set; }
        public bool CanAccess { get; set; }
    }

    public class PermissionsListViewModels
    {
        public MenuViewModels MenuGroup { get; set; }
        public IEnumerable<MenuViewModels> MenuItemsList { get; set; }
        public int? GroupId { get; set; }
    }

    public class MenuViewModels
    {
        public MenuItems MenuItem { get; set; }
        public IEnumerable<PermissionsViewModels> PagePermissions { get; set; }
    }


    public class AddPermissionsViewModels
    {
        public AddPermissionsViewModels()
        {
        }
        public IEnumerable<MenuItems> MenuItemsGroupList { get; set; }

        public IEnumerable<EnumListViewModel> PermissionsEnum
        {
            get
            {
                return ((IEnumerable<Enumerations.Permissions>)Enum.GetValues(typeof(Enumerations.Permissions))).Select(c => new EnumListViewModel() { Value = (byte)c, Name = c.ToString() }).ToList();
            }
        }

        public int? GroupId { get; set; }
        public int MenuItemId { get; set; }
        public int PermissionId { get; set; }
        public List<MenuItemsListViewModels> MenuItemsList { get; set; }
        public List<PagePermissions> PermissionsList { get; set; }
        public PagePermissions Permission { get; set; }
    }
}