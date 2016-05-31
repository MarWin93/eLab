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
        public void Post([FromBody]CoursesJson course)
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
