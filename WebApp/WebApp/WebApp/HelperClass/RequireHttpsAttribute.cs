using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.HelperClass
{
    public class RequireHttpsAttribute : System.Web.Mvc.RequireHttpsAttribute
    {
        protected override void HandleNonHttpsRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            base.HandleNonHttpsRequest(filterContext);
            
            if (filterContext.HttpContext.Request.Url.Host.ToLower().Equals("localhost"))
            {
                var result = (RedirectResult)filterContext.Result;
                var uri = new UriBuilder(result.Url);
                uri.Port = 44368;

                filterContext.Result = new RedirectResult(uri.ToString());
            }
        }
    }
}