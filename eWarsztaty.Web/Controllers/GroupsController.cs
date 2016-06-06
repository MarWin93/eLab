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
        public GroupsJson Get(int id)
        {
            if (id > 0)
            {
                var groupRepository = new GroupRepository();
                return groupRepository.GetGroupById(id);
            }
            else
            {
                var newGroup = new GroupsJson();
                return newGroup;
            }
        }

        // POST api/groups
        public IHttpActionResult Post([FromBody]GroupsJson group)
        {
            var groupRepository = new GroupRepository();
            groupRepository.SaveGroup(group);
            //return Created<CoursesJson>(Request.RequestUri + )
            return Ok();
        }

        // PUT api/groups/5
        public IHttpActionResult Put(int id, [FromBody]GroupsJson course)
        {
            var groupRepository = new GroupRepository();
            groupRepository.SaveCourse(id, course);
            return Ok();
        }

        // DELETE api/groups/5
        public void Delete(int id)
        {
            var groupRepository = new GroupRepository();
            groupRepository.DeleteCourse(id);
        }
    }
}
