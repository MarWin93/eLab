using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Models.JsonModels;
using eWarsztaty.Web.Models.ViewModels;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class CourseRepository
    {
        private eWarsztatyContext _db = new eWarsztatyContext();

        public IEnumerable<CoursesJson> GetAllCourses()
        {
            var courses = _db.Courses.Include("Topics").Include("Prowadzacy").ToList();
            var coursesJson = Mapper.Map<IEnumerable<Course>, IEnumerable<CoursesJson>>(courses);
            return coursesJson;
        }
    }
}