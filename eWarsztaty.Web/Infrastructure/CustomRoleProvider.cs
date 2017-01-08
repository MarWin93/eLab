using eWarsztaty.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace eWarsztaty.Web.Infrastructure
{
    public class CustomRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            using (var usersContext = new eLabContext())
            {
                var user = usersContext.Uzytkownicy.FirstOrDefault(u => u.Login == username);
                if (user == null)
                    return false;
                return user.UzytkownicyRole != null && user.UzytkownicyRole.Select(u => u.Role).Any(r => r.Nazwa == roleName);
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var usersContext = new eLabContext())
            {
                var user = usersContext.Uzytkownicy.FirstOrDefault(u => u.Login == username);
                if (user == null)
                    return new string[] { };
                return user.UzytkownicyRole == null ? new string[] { } : user.UzytkownicyRole.Select(u => u.Role).Select(u => u.Nazwa).ToArray();
            }
        }

        public override void CreateRole(string roleName)
        {
            using (var usersContext = new eLabContext())
            {
                usersContext.AddRole(roleName);
            }
        }

        public void CreateRole(string roleName, string roleDescription = "")
        {
            using (var usersContext = new eLabContext())
            {
                usersContext.AddRole(roleName, roleDescription);
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            using (var usersContext = new eLabContext())
            {
                var rola = usersContext.Role.FirstOrDefault(x => x.Nazwa == roleName);
                if (rola != null)
                    return true;
                else
                    return false;
            }
        }

        public static void AddUserToRole(string userName, string roleName)
        {
            using (var usersContext = new eLabContext())
            {
                var user = usersContext.GetUser(userName);
                var role = usersContext.GetRola(roleName);
                usersContext.AddUserRole(user.UzytkownikId, role.RolaId);
            }
        }

        public static int GetUserId(string userName)
        {
            using (var usersContext = new eLabContext())
            {
                var user = usersContext.GetUser(userName);
                return user.UzytkownikId;
            }
        }


        public static void AddRolesToUzytkownik(List<int> listaId, string userName)
        {
            using (var usersContext = new eLabContext())
            {
                usersContext.RemoveAllRolesFromUzytkownik(userName);
                usersContext.AddRolesUzytkownik(listaId, userName);
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            using (var usersContext = new eLabContext())
            {
                return usersContext.Role.Select(r => r.Nazwa).ToArray();
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}