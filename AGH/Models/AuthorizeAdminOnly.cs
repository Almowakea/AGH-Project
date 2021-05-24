using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AGH.Models
{
    public class AuthorizeAdminOnly : AuthorizeAttribute
    {
        // GET: AuthorizeAdminOnly
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //Unauthorized users IDs, Instructor = 1, Assistant = 2, Student = 3
            var roleIDs = new List<int>() { 4 }; // Admin Role ID = 4

            // If user is not ADMIN, handle accordingly
            if (!roleIDs.Contains((Int32)HttpContext.Current.Session["UserRoleID"]))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                    { "Action", "Contact" },
                    { "Controller", "Home" }
                   //{"QueryString","SomeValue" }
            });
            }
        }
    }
}