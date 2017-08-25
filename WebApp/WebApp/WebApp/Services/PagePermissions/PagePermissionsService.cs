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
    public class PagePermissionsService : Service<PagePermissions>, IPagePermissionsService
    {
        private readonly IRepositoryAsync<PagePermissions> _repository;
        public PagePermissionsService(IRepositoryAsync<PagePermissions> repository) : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<PermissionsListViewModels> GetAllMenuPermissionsByRoleId(int roleId)
        {
            return _repository.GetAllMenuPermissionsByRoleId(roleId);
        }

        public IEnumerable<PagePermissions> GetAllPermissionsByResourceId(int resourceId)
        {
            return _repository.GetAllPermissionsByResourceId(resourceId);
        }
    }
}