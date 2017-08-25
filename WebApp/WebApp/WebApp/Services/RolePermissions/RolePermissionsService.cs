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
    public class RolePermissionsService : Service<RolePermissions>, IRolePermissionsService
    {
        private readonly IRepositoryAsync<RolePermissions> _repository;
        public RolePermissionsService(IRepositoryAsync<RolePermissions> repository) : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<PermissionsListViewModels> GetAllMenuPermissionsByRoleId(int roleId)
        {
            return _repository.GetAllMenuPermissionsByRoleId(roleId);
        }
    }
}