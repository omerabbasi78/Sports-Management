using Repository.Pattern;
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
    public interface IMenuItemsService : IService<MenuItems>
    {
        IEnumerable<MenuItems> GetAllGroups();
        IEnumerable<MenuItemsListViewModels> GetAllResourcesByGroupId(int groupId=0);
    }
}
