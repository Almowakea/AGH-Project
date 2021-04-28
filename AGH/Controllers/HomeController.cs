using AGH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AGH.ViewModel;
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
        public ActionResult Login(User objUser)
        {
            if (ModelState.IsValid)
            {
                using (AGH_DBContext db = new AGH_DBContext())
                {
                    var obj = db.Users.Where(a => a.User_ID.Equals(objUser.User_ID) && a.User_Password.Equals(objUser.User_Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.User_ID;
                        Session["UserRoleID"] = obj.User_Type.ID;
                        Session["UserName"] = obj.User_First_Name.ToString() + " " + obj.User_Last_Name.ToString();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(objUser);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); //Clear & Terminate Session Object
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Index()
        {
            var show = new ShowLinks();

            //if ((Int32)Session["UserRoleID"] == 4)
            //{
            //    show.showHiddenLinks = true;
            //}

            //else
            //{
            //    show.showHiddenLinks = false;
            //}

            return View(show);

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