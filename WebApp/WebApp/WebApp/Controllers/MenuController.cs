using Repository.Pattern;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class MenuController : BaseController
    {
        Result<int> saveResult = new Result<int>();
        private IRolePermissionsService _rolesPermissionService;
        private IPagePermissionsService _pagePermissionsService;
        private IMenuItemsService _menuItemsService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public MenuController(IUnitOfWorkAsync unitOfWork,IRolePermissionsService rolesPermissionService, IPagePermissionsService pagePermissionsService, IMenuItemsService menuItemsService)
        {
            _rolesPermissionService = rolesPermissionService;
            _pagePermissionsService = pagePermissionsService;
            _menuItemsService = menuItemsService;
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index(int id = 0, int rid = 0)
        {
            MenuItemsViewModels model = new MenuItemsViewModels();
            model.MenuItemsGroupList = _menuItemsService.GetAllGroups();
            if (id > 0)
            {
                model.MenuItem = _menuItemsService.Find(id).data;
            }
            else
            {
                model.MenuItem = new MenuItems();
                model.MenuItem.GroupId = rid;
            }
            if (rid > 0)
            {
                model.MenuItemsList = _menuItemsService.GetAllResourcesByGroupId(rid);
            }
            else
            {
                model.MenuItemsList = _menuItemsService.GetAllResourcesByGroupId();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(MenuItemsViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.MenuItem.MenuItemId > 0)
            {
                model.MenuItem.ObjectState = ObjectState.Modified;
                _menuItemsService.InsertOrUpdateGraph(model.MenuItem);
                saveResult = _unitOfWork.SaveChanges();
                if (saveResult.success)
                {
                    //List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
                    //foreach (string cacheKey in cacheKeys)
                    //{
                    //    MemoryCache.Default.Remove(cacheKey);
                    //}
                    TempData["SuccessMessage"] = "Updated successfully.";
                    RouteData.Values.Remove("id");
                }
                else
                {
                    AddErrors(saveResult.errors, saveResult.ErrorMessage);
                    return View(model);
                }
            }
            else
            {
                model.MenuItem.ObjectState = ObjectState.Added;
                _menuItemsService.InsertOrUpdateGraph(model.MenuItem);
                saveResult = _unitOfWork.SaveChanges();
                if (saveResult.success)
                {
                    TempData["SuccessMessage"] = "Added successfully.";
                }
                else
                {
                    AddErrors(saveResult.errors, saveResult.ErrorMessage);
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            _menuItemsService.Delete(id);
            saveResult = _unitOfWork.SaveChanges();
            if (saveResult.success)
            {
                //List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
                //foreach (string cacheKey in cacheKeys)
                //{
                //    MemoryCache.Default.Remove(cacheKey);
                //}
                TempData["SuccessMessage"] = "Deleted successfully.";
            }
            else
            {
                AddErrors(saveResult.errors, saveResult.ErrorMessage);
            }
            RouteData.Values.Remove("id");
            return RedirectToAction("Index");
        }
    }
}