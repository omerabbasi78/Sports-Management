using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Services
{
    public class RolesService : Service<Roles>, IRolesService
    {
        public RolesService(IRepositoryAsync<Roles> repository) : base(repository)
        {
        }
    }
}