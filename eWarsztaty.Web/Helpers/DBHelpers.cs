using eWarsztaty.Web.Infrastructure;
using System.Linq;
using eWarsztaty.Domain;

namespace eWarsztaty.Web.Helpers
{
    public class DBHelpers
    {
        public static int GetUserId(string userName)
        {
            using (var usersContext = new eWarsztatyContext())
            {
                var user = usersContext.GetUser(userName);
                return user.UzytkownikId;
            }
        }

        public static Rola GetRoleByUserId(int userId)
        {
            using (var usersContext = new eWarsztatyContext())
            {
                return userId <= 0 ? null : usersContext.UzytkownicyRole.Include("Rola").FirstOrDefault(x => x.UzytkownikId == userId).Rola;
            }
        }
    }
}