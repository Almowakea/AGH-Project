using System.Web;
using AGH.Models;
using System.Web.Mvc;

namespace AGH
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //CustomAuthorize redirects to login page
            filters.Add(new CustomAuthorize());
        }
    }
}
