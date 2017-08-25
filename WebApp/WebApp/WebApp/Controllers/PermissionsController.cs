using Repository.Pattern;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.HelperClass;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class PermissionsController : Controller
    {
        Result<int> saveResult = new Result<int>();
        private IRolePermissionsService _rolesPermissionService;
        private IPagePermissionsService _pagePermissionsService;
        private IMenuItemsService _menuItemsService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public PermissionsController(IUnitOfWorkAsync unitOfWork, IRolePermissionsService rolesPermissionService, IPagePermissionsService pagePermissionsService, IMenuItemsService menuItemsService)
        {
            _rolesPermissionService = rolesPermissionService;
            _pagePermissionsService = pagePermissionsService;
            _menuItemsService = menuItemsService;
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index(int rgid = 0, int rid = 0, int id = 0)
        {
            AddPermissionsViewModels model = new AddPermissionsViewModels();
            model.Permission = new PagePermissions();
            model.Permission.MenuItemId = rid;
            model.GroupId = rgid;
            model.MenuItemId = rid;
            model.MenuItemsGroupList = _menuItemsService.GetAllGroups();
            if (rid > 0)
            {
                var pList = _pagePermissionsService.GetAllPermissionsByResourceId(rid);
                model.PermissionsList = pList == null ? new List<PagePermissions>() : pList.ToList();
            }
            var rList = _menuItemsService.GetAllResourcesByGroupId(rgid);
            model.MenuItemsList = rList == null ? new List<MenuItemsListViewModels>() : rList.ToList();
            if (id > 0)
            {
                var permission=_pagePermissionsService.QueryableCustom().Where(w => w.PagePermissionId == id && w.IsActive).FirstOrDefault();
                model.Permission = permission == null?new PagePermissions(): permission;
                model.PermissionId = model.Permission.PermissionId;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AddPermissionsViewModels model)
        {
            model.Permission.MenuItemId = model.MenuItemId;
            model.Permission.PermissionId = model.PermissionId;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Enumerations.Permissions enumPermission = (Enumerations.Permissions)model.PermissionId;
            model.Permission.PermissionText = enumPermission.ToString();
            if (model.Permission.PagePermissionId > 0)
            {
                model.Permission.ObjectState = ObjectState.Modified;
                _pagePermissionsService.InsertOrUpdateGraph(model.Permission);
                saveResult= _unitOfWork.SaveChanges();
                if (saveResult.success)
                {
                    TempData["SuccessMessage"] = "Updated successfully.";
                    RouteData.Values.Remove("id");
                }
            }
            else
            {
                model.Permission.ObjectState = ObjectState.Added;
                _pagePermissionsService.InsertOrUpdateGraph(model.Permission);
                saveResult = _unitOfWork.SaveChanges();
                if (saveResult.success)
                {
                    TempData["SuccessMessage"] = "Added successfully.";
                }
            }
            return RedirectToAction("Index", new { rgid = model.GroupId, rid = model.Permission.MenuItemId, id = string.Empty });
        }

        public ActionResult Delete(int rgid, int rid = 0, int id = 0)
        {
            _pagePermissionsService.Delete(id);
            var result = _unitOfWork.SaveChanges();
            if (result.success)
            {
                TempData["SuccessMessage"] = "Deleted successfully.";
            }
            return RedirectToAction("Index", new { rgid = rgid, rid = rid, id = string.Empty });
        }

        public ActionResult RolePermissions(int id)
        {
            var data = _pagePermissionsService.GetAllMenuPermissionsByRoleId(id);
            return View(data);
        }

        [HttpPost]
        public ActionResult SaveRolePermissions(List<int> values, int id)
        {
            //var permissions = _pagePermissionsService.UpdateRolePermssion(values, id);

            //List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            //foreach (string cacheKey in cacheKeys)
            //{
            //    MemoryCache.Default.Remove(cacheKey);
            //}
            //TempData["SuccessMessage"] = "Saved successfully.";
            return RedirectToAction("RolePermissions", new { id = id });
        }
    }
}