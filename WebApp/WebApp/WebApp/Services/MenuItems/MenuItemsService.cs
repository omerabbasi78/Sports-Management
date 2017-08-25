using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public class MenuItemsService : Service<MenuItems>, IMenuItemsService
    {
        private readonly IRepositoryAsync<MenuItems> _repository;
        public MenuItemsService(IRepositoryAsync<MenuItems> repository) : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<MenuItems> GetAllGroups()
        {
            return _repository.GetAllGroups();
        }

        public IEnumerable<MenuItemsListViewModels> GetAllResourcesByGroupId(int groupId=0)
        {
            return _repository.GetAllResourcesByGroupId(groupId);
        }
    }
}