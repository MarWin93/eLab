using eWarsztaty.Web.Infrastructure.Repositories;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using eWarsztaty.Web.Infrastructure.Repositories;

namespace eWarsztaty.Web.Controllers
{
    public class ClassesController : ApiController
    {
        // GET api/classes
        public IEnumerable<ClassJson> Get()
        {
            var classRepository = new ClassRepository();
            return classRepository.GetAllClasses();
        }

        // GET api/classes/5
        public ClassJson Get(int id)
        {
            if (id > 0)
            {
                var classRepository = new ClassRepository();
                return classRepository.GetClassById(id);
            }
            else
            {
                var newClass = new ClassJson();
                return newClass;
            }
        }

        // POST api/classes
        public void Post([FromBody]string value)
        {
        }

        // PUT api/classes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/classes/5
        public void Delete(int id)
        {
        }

        [Route("api/classes/{id}/close")]
        [HttpGet]
        public IHttpActionResult CloseClass(int id)
        {
            var classRepository = new ClassRepository();
            classRepository.CloseCourse(id);
            return Ok();
        }
    }
}
