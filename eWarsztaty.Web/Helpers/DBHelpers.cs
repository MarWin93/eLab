using eWarsztaty.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}