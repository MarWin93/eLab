using System.Collections.Generic;
using System.Web.Http;
using eWarsztaty.Web.Infrastructure.Repositories;
using eWarsztaty.Web.Models.JsonModels;

namespace eWarsztaty.Web.Controllers
{
    public class CoursesController : ApiController
    {
        readonly CourseRepository _courseRepository = new CourseRepository();
        // GET api/courses
        public IEnumerable<CoursesJson> Get()
        {
            return _courseRepository.GetAllCourses();
        }

        // GET api/courses/5
        public CoursesJson Get(int id)
        {
            if (id > 0)
            {
                return _courseRepository.GetCourseById(id);
            }
            var newCourse = new CoursesJson();
            return newCourse;
        }

        // POST api/courses
        public IHttpActionResult Post([FromBody]CoursesJson course)
        {
            _courseRepository.SaveCourse(course);
            return Ok();
        }

        // PUT api/courses/5
        public IHttpActionResult Put(int id, [FromBody]CoursesJson course)
        {
            _courseRepository.SaveCourse(id, course);
            return Ok();
        }

        // DELETE api/courses/5
        public void Delete(int id)
        {
            _courseRepository.DeleteCourse(id);
        }

        [Route("api/courses/{id}/close")]
        [HttpGet]
        public IHttpActionResult CloseCourse(int id)
        {
            _courseRepository.CloseCourse(id);
            return Ok();
        }
    }
}
