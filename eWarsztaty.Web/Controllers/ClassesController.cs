using eWarsztaty.Web.Infrastructure.Repositories;
using eWarsztaty.Web.Models.JsonModels;
using System.Collections.Generic;
using System.Web.Http;

namespace eWarsztaty.Web.Controllers
{
    public class ClassesController : ApiController
    {
        readonly ClassRepository _classRepository = new ClassRepository();
        // GET api/classes
        public IEnumerable<ClassJson> Get()
        {
            return _classRepository.GetAllClasses();
        }

        // GET api/classes/5
        public ClassJson Get(int id)
        {
            if (id > 0)
            {
                return _classRepository.GetClassById(id);
            }
            var newClass = new ClassJson();
            return newClass;
        }

        // POST api/classes
        public IHttpActionResult Post([FromBody]ClassJson classJson)
        {
            _classRepository.SaveClass(classJson);
            return Ok();
        }

        // PUT api/classes/5
        public IHttpActionResult Put(int id, [FromBody]ClassJson classJson)
        {
            _classRepository.SaveClass(id, classJson);
            return Ok();
        }

        // DELETE api/classes/5
        public IHttpActionResult Delete(int id)
        {
            _classRepository.DeleteClass(id);
            return Ok();
        }

        [Route("api/classes/{id}/close")]
        [HttpGet]
        public IHttpActionResult CloseClass(int id)
        {
            _classRepository.CloseClass(id);
            return Ok();
        }
    }
}
