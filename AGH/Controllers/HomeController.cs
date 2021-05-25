using System;
using System.Linq;
using System.Web.Mvc;
using AGH.Models.DTO;
using System.Web.Security;
using AGH.Services;

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
            //var error = ModelState.Values;
            try
            {
                if (ModelState.IsValid)
                {
                    using (AGH_DBContext db = new AGH_DBContext())
                    {
                        var obj = db.Users.Where(a => a.User_ID.Equals(objUser.User_ID)).FirstOrDefault();

                        if(obj.Is_User_Deleted == false)
                        {
                            // Checks if entered password matches the password in DB
                            if (HashPasswordService.CompareHash(objUser.User_Password, obj.User_Password_Salt, obj.User_Password))
                            {
                                Session["UserID"] = obj.User_ID;
                                Session["UserRoleID"] = obj.User_Type.ID;
                                Session["UserName"] = obj.User_First_Name.ToString() + " " + obj.User_Last_Name.ToString();

                                return RedirectToAction("Index");
                            }

                            ViewBag.LoginErrorMessage = "Please check your login credentials and try again";
                            return View("Login");
                        }

                        ViewBag.LoginErrorMessage = "Your user has been deactivated. GET LOST!";
                        return View("Login");
                    }
                }

                return View(objUser);
            }

            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
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