using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.ViewModels
{
    public class MenuItemsViewModels
    {
        IMenuItemsService _menuItemService;
        public MenuItemsViewModels()
        {
        }
        public MenuItemsViewModels(IMenuItemsService menuItemService)
        {
            _menuItemService = menuItemService;
        }
        public IEnumerable<MenuItems> MenuItemsGroupList { get; set; }
        public IEnumerable<MenuItemsListViewModels> MenuItemsList { get; set; }
        public MenuItems MenuItem { get; set; }
    }
    public class MenuItemsListViewModels
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string GroupName { get; set; }
        public string IconClass { get; set; }
        public bool IsParent { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class RenderMenuListViewModels
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string GroupName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string IconClass { get; set; }
        public bool IsParent { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<int> SortOrder { get; set; }

    }
}