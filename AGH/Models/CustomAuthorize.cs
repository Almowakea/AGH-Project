using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AGH.Models
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        // GET: CustomAuthorize
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // If user is unAuthorized, handle accordingly
            if (HttpContext.Current.Session["UserID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                    { "Action", "Login" },
                    { "Controller", "Home" }
                   //{"QueryString","SomeValue" }
            });
            }
        }
    }
}