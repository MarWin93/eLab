using eWarsztaty.Web.Infrastructure.Repositories;
using eWarsztaty.Web.Models.JsonModels;
using System.Collections.Generic;
using System.Web.Http;

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
        public IHttpActionResult Post([FromBody]ClassJson classJson)
        {
            var classRepository = new ClassRepository();
            classRepository.SaveClass(classJson);
            return Ok();
        }

        // PUT api/classes/5
        public IHttpActionResult Put(int id, [FromBody]ClassJson classJson)
        {
            var classRepository = new ClassRepository();
            classRepository.SaveClass(id, classJson);
            return Ok();
        }

        // DELETE api/classes/5
        public IHttpActionResult Delete(int id)
        {
            var classRepository = new ClassRepository();
            classRepository.DeleteClass(id);
            return Ok();
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
