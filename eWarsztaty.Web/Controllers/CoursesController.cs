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
        public CoursesJson Get(int id)
        {
            if (id > 0)
            {
                var courseRepository = new CourseRepository();
                return courseRepository.GetCourseById(id);
            }
            else
            {
                var newCourse = new CoursesJson();
                return newCourse;
            }
        }

        // POST api/courses
        public IHttpActionResult Post([FromBody]CoursesJson course)
        {
            var courseRepository = new CourseRepository();
            courseRepository.SaveCourse(course);
            //return Created<CoursesJson>(Request.RequestUri + )
            return Ok();
        }

        // PUT api/courses/5
        public IHttpActionResult Put(int id, [FromBody]CoursesJson course)
        {
            var courseRepository = new CourseRepository();
            courseRepository.SaveCourse(id, course);
            return Ok();
        }

        // DELETE api/courses/5
        public void Delete(int id)
        {
            var courseRepository = new CourseRepository();
            courseRepository.DeleteCourse(id);
        }
    }
}
