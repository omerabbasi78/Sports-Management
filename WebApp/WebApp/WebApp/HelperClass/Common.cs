using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.IO;
using System.Web.Mvc;
using WebApp.ViewModels;
using System.Web.Routing;

namespace WebApp.HelperClass
{
    public class Common
    {
        public static UserInfo CurrentUser
        {
            get
            {
                UserInfo user = new UserInfo();
                try
                {
                    
                    var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                    user.UserName = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
                    user.Email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
                    user.Id = long.Parse(claimsIdentity.FindFirst("UserId").Value);
                    user.RoleName = claimsIdentity.FindFirst("RoleName").Value;
                    user.RoleId = Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value);
                    return user;
                }
                catch (Exception e)
                {
                    return user;
                }
            }
        }

        public static UrlHelper GetUrlHelper()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            return new UrlHelper(new RequestContext(httpContext, CurrentRoute(httpContext)));
        }
        public static RouteData CurrentRoute(HttpContextWrapper httpContext)
        {
            return RouteTable.Routes.GetRouteData(httpContext);
        }

        public static string RenderRazorViewToString(string viewName, object model, ControllerBase controllerBase)
        {

            controllerBase.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerBase.ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(controllerBase.ControllerContext, viewResult.View,
                                            controllerBase.ViewData, controllerBase.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerBase.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}