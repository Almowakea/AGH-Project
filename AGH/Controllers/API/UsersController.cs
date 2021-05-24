using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Http;
using System.Net;

namespace AGH.Controllers.API
{
    public class UsersController : ApiController
    {
        private AGH_DBContext _dbContext;

        public UsersController()
        {
            try
            {
                _dbContext = new AGH_DBContext();
            }

            catch (Exception)
            {
                throw new Exception("Problem occured with DB");
            }
        }
        
        // Get:/api/Users
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            try
            {
                var users = _dbContext.Users.Where(u => u.Is_User_Deleted != true);
                return Ok(users.ToList());
            }

            catch (Exception)
            {
                throw new Exception("Couldn't get users list");
            }

        }

        // Get:/api/Users/id
        [HttpGet]
        public IHttpActionResult GetUser(int id)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(n => n.ID == id);

                if (user == null || user.Is_User_Deleted == true)
                {
                   return NotFound();
                }

                return Ok(user);
            }
            catch (Exception)
            {
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
                throw new Exception("Couldn't get the user");
            }
            
        }

        // Post:/api/users
        [HttpPost]
        public User CreateUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BadRequest();
                }
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return user;
            }
            catch (Exception)
            {
                throw new Exception("Some message here");
            }
            
        }

        // Put:/api/Users/id
        [HttpPut]
        public void UpdateUser(int id, User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BadRequest();
                }
                var userInDB = _dbContext.Users.Find(id);

                if (userInDB == null || user.Is_User_Deleted == true)
                {
                    NotFound();
                }

                userInDB.User_First_Name = user.User_First_Name;
                userInDB.User_Last_Name = user.User_Last_Name;
                userInDB.User_Type_ID = user.User_Type_ID;
                userInDB.User_Phone_Number = user.User_Phone_Number;
                userInDB.User_Email = user.User_Email;
                userInDB.User_ID = user.User_ID;
                userInDB.User_Password = user.User_Password;

                _dbContext.SaveChanges();
            }

            catch (Exception)
            {
                throw new Exception("Some Exception message here");
            }


        }

        // Delete:/api/customers/id
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var userInDB = _dbContext.Users.Find(id);

            try
            {
                if (userInDB == null || userInDB.Is_User_Deleted == true)
                {
                    NotFound();
                }
                userInDB.Is_User_Deleted = true;
                _dbContext.Users.Remove(userInDB);
                _dbContext.SaveChanges();

            }
            catch (Exception)
            {
                throw new Exception("Some Exception message here");
            }




        }
    }
}