using Repository.Pattern;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class RolesController : Controller
    {
        Result<int> saveResult = new Result<int>();
        private IRolesService _rolesService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public RolesController(IRolesService rolesService, IUnitOfWorkAsync unitOfWork)
        {
            _rolesService = rolesService;
            _unitOfWork = unitOfWork;
        }
        // GET: Roles
        public ActionResult Index()
        {
            return View(_rolesService.Queryable().data);
        }

        public ActionResult Create()
        {
            Result<Roles> result = new Result<Roles>();
            Roles role = new Roles();
            var da = _rolesService.Queryable();
            //role.RoleName = "Admin";
            result = _rolesService.Insert(role);
            saveResult = _unitOfWork.SaveChanges();
            return View();
        }
    }
}