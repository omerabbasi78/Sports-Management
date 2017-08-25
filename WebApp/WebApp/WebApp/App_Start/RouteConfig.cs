﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "PermissionsPage",
                url: "Permissions/Index/{rgid}/{rid}/{id}",
                defaults: new { controller = "Permissions", action = "Index", rid = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MenuPage",
                url: "Menu/Index/{id}/{rid}",
                defaults: new { controller = "Menu", action = "Index", rid = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
