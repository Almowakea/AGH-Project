using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.Entity;

namespace AGH.Models
{
    public class RoleDetector : RoleProvider
    {
        public override bool IsUserInRole(string userName, string roleName)
        {
            try
            {
                using (AGH_DBContext db = new AGH_DBContext())
                {
                    var userRoles = (from U in db.Users
                                     join T in db.User_Type
                                     on U.User_Type_ID equals T.ID
                                     where U.User_First_Name == userName
                                     select T.Type).ToArray();
                    if (userRoles.Contains(roleName))
                        return true;

                    return false;
                }
            }
            catch
            {
                throw new NotImplementedException();
            }

        }
        public override string[] GetRolesForUser(string userName)
        {
            using (AGH_DBContext db = new AGH_DBContext())
            {
                var userRoles = (from U in db.Users
                                join T in db.User_Type
                                on U.User_Type_ID equals T.ID
                                where U.User_First_Name == userName
                                select T.Type).ToArray();
                return userRoles;
            }
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}