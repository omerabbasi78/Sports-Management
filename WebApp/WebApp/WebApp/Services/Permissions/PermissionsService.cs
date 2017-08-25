using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Services
{
    public class PermissionsService : Service<Permissions>, IPermissionsService
    {
        private readonly IRepositoryAsync<Permissions> _repository;
        public PermissionsService(IRepositoryAsync<Permissions> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}