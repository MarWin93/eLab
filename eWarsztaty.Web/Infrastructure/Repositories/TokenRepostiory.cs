using System;
using eWarsztaty.Domain;
using eWarsztaty.Web.Helpers;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class TokenRepostiory
    {
        private eWarsztatyContext _db = new eWarsztatyContext();

        public Uzytkownik GetUserByCredentials(string username, string password)
        {
            var user = _db.GetUser(username, password);
            return user;
        }

        public Tuple<Uzytkownik, Rola> GetUserAndHisRoleByCredentials(string username, string password)
        {
            
            var user = _db.GetUser(username, password);
            var role = DBHelpers.GetRoleByUserId(user != null ? user.UzytkownikId : 0);
            return new Tuple<Uzytkownik, Rola>(user, role);
        }
    }
}