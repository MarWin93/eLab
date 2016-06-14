using eWarsztaty.Domain;

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
    }
}