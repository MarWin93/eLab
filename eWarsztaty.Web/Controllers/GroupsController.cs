using eWarsztaty.Web.Infrastructure.Repositories;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eWarsztaty.Web.Controllers
{
    public class GroupsController : ApiController
    {
        // GET api/groups
        public IEnumerable<GroupsJson> Get()
        {
            var groupRepository = new GroupRepository();
            return groupRepository.GetAllGroups();
        }

        // GET api/groups/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/groups
        public void Post([FromBody]string value)
        {
        }

        // PUT api/groups/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/groups/5
        public void Delete(int id)
        {
        }
    }
}
