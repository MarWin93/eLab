using System.Collections.Generic;
using System.Web.Http;
using eWarsztaty.Web.Models.JsonModels;
using eWarsztaty.Web.Infrastructure.Repositories;

namespace eWarsztaty.Web.Controllers
{
    public class TOKENController : ApiController
    {
        // GET: api/TOKEN
        public IEnumerable<string> Get()
        {
            return new string[]{"aaa"};
        }

        // GET: api/TOKEN/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TOKEN
        public TokenJson Post([FromBody]LoginCredentialsJson value)
        {
            var tokenRepository = new TokenRepostiory();
            var userAndRole = tokenRepository.GetUserAndHisRoleByCredentials(value.Username, value.Password);
            if (userAndRole.Item1 != null)
            {
                return new TokenJson
                {
                    Id = userAndRole.Item1.UzytkownikId, Username = userAndRole.Item1.Login,
                    RoleId = userAndRole.Item2 != null ? userAndRole.Item2.RolaId : 0,
                    RoleName = userAndRole.Item2 != null ? userAndRole.Item2.Nazwa : null
                };
            }

            BadRequest();
            return  new TokenJson();
        }

        // PUT: api/TOKEN/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TOKEN/5
        public void Delete(int id)
        {
        }
    }
}
