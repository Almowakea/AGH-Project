using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace AGH.Controllers
{
    public class ApiViewsController : Controller
    {
        private AGH_DBContext _dbContext = new AGH_DBContext();


        // GET: ApiViews
        public ActionResult Index()
        {
            var users = _dbContext.Users.Include(u => u.User_Type);
            return View(users.ToList());
        }

            // Get:/api/Users
            [HttpGet]
            public IEnumerable<User> GetUsers()
            {
                var users = _dbContext.Users.ToList();
                return users;
            }

            // Get:/api/Users/id
            [HttpGet]
            public User GetUser(int id)
            {
                var user = _dbContext.Users.FirstOrDefault(n => n.ID == id);


                return user;
            }

            // Post:/api/users
            [HttpPost]
            public User CreateUser(User user)
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return user;
            }

            // Put:/api/Users/id
            [HttpPut]
            public void UpdateUser(int id, User user)
            {
                var userInDB = _dbContext.Users.Find(id);

                

                userInDB.User_First_Name = user.User_First_Name;
                userInDB.User_Last_Name = user.User_Last_Name;
                userInDB.User_Type_ID = user.User_Type_ID;
                userInDB.User_Phone_Number = user.User_Phone_Number;
                userInDB.User_Email = user.User_Email;
                userInDB.User_ID = user.User_ID;
                userInDB.User_Password = user.User_Password;

                _dbContext.SaveChanges();

            }

            // Delete:/api/customers/id
            [HttpDelete]
            public void DeleteCustomer(int id)
            {
                var userInDB = _dbContext.Users.Find(id);


                _dbContext.Users.Remove(userInDB);
                _dbContext.SaveChanges();
            }
        }
    }
