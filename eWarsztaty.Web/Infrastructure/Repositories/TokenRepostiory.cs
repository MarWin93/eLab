using System;
using eWarsztaty.Domain;
using eWarsztaty.Web.Helpers;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class TokenRepostiory
    {
        private eLabContext _db = new eLabContext();

        public User GetUserByCredentials(string username, string password)
        {
            var user = _db.GetUser(username, password);
            return user;
        }

        public Tuple<User, Role> GetUserAndHisRoleByCredentials(string username, string password)
        {
            
            var user = _db.GetUser(username, password);
            var role = DBHelpers.GetRoleByUserId(user != null ? user.UzytkownikId : 0);
            return new Tuple<User, Role>(user, role);
        }
    }
}