
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using WebApp.Models;

namespace newfrontiemvc.Repositories
{
   public class RolesQuery : QueryObject<Roles>
    {
        public RolesQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.RoleName.Contains(search) || x.RoleId.ToString().Contains(search) );
            return this;
        }
    }
}



