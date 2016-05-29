using System.Collections.Generic;
using System.Web.Http;
using eWarsztaty.Domain;
using eWarsztaty.Web.Infrastructure.Repositories;
using eWarsztaty.Web.Models.JsonModels;

namespace eWarsztaty.Web.Controllers
{
    public class CoursesController : ApiController
    {
        // GET api/courses
        public IEnumerable<CoursesJson> Get()
        {
            var courseRepository = new CourseRepository();
            return courseRepository.GetAllCourses();
        }

        // GET api/courses/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/courses
        public void Post([FromBody]string value)
        {
        }

        // PUT api/courses/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/courses/5
        public void Delete(int id)
        {
        }
    }
}
