using eWarsztaty.Web.Infrastructure;
using System.Linq;
using eWarsztaty.Domain;

namespace eWarsztaty.Web.Helpers
{
    public class DBHelpers
    {
        public static int GetUserId(string userName)
        {
            using (var usersContext = new eLabContext())
            {
                var user = usersContext.GetUser(userName);
                return user.UzytkownikId;
            }
        }

        public static Role GetRoleByUserId(int userId)
        {
            using (var usersContext = new eLabContext())
            {
                return userId <= 0 ? null : usersContext.UzytkownicyRole.Include("Rola").FirstOrDefault(x => x.UzytkownikId == userId).Role;
            }
        }
    }
}