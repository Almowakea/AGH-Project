using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Http;

namespace AGH.Controllers.API
{
    public class UsersController : ApiController
    {
        private AGH_DBContext _dbContext;

        public UsersController()
        {
            _dbContext = new AGH_DBContext();
        }
        
        // Get:/api/Users
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {

            var users = _dbContext.Users;
            return users.ToList();
        }

        // Get:/api/Users/id
        [HttpGet]
        public User GetUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(n => n.ID == id);

            if(user == null)
            {
                NotFound();
            }

            return user;
        }

        // Post:/api/users
        [HttpPost]
        public User CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                BadRequest();
            }
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }

        // Put:/api/Users/id
        [HttpPut]
        public void UpdateUser(int id, User user)
        {
            if (ModelState.IsValid)
            {
                BadRequest();
            }
            var userInDB = _dbContext.Users.Find(id);

            if(userInDB == null)
            {
                NotFound();
            }

            userInDB.User_First_Name = user.User_First_Name;
            userInDB.User_Last_Name = user.User_Last_Name;
            userInDB.User_Type_ID = user.User_Type_ID;
            userInDB.User_Phone_Number = user.User_Phone_Number;
            userInDB.User_Email= user.User_Email;
            userInDB.User_ID = user.User_ID;
            userInDB.User_Password = user.User_Password;
            
            _dbContext.SaveChanges();

        }

        // Delete:/api/customers/id
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var userInDB = _dbContext.Users.Find(id);

            if (userInDB == null)
            {
                NotFound();
            }

            _dbContext.Users.Remove(userInDB);
            _dbContext.SaveChanges();
        }
    }
}