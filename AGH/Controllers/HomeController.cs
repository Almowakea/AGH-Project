using AGH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AGH.ViewModel;
using AGH.Models.DTO;
using System.Web.Security;

namespace AGH.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(userLogin objUser)

        {
            var error = ModelState.Values;
            if (ModelState.IsValid)
            {
                using (AGH_DBContext db = new AGH_DBContext())
                {
                    var obj = db.Users.Where(a => a.User_ID.Equals(objUser.User_ID) && a.User_Password.Equals(objUser.User_Password) && a.Is_User_Deleted == false).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.User_ID;
                        Session["UserRoleID"] = obj.User_Type.ID;
                        Session["UserName"] = obj.User_First_Name.ToString() + " " + obj.User_Last_Name.ToString();
                    }
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Message = "Something wrong happened";
            return View(objUser);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); //Clear & Terminate Session Object
            return RedirectToAction("Login");
        }

        public ActionResult Index()
        {
            //var show = new ShowLinks();

            return View();

        }
        public ActionResult About()
        {
            ViewBag.Message = "This is how we do it here!";
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();

        }
    }
}