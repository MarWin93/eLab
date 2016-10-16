using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Helpers;
using eWarsztaty.Web.Models.JsonModels;

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

        public CoursesJson GetCourseById(int id)
        {
            var course = _db.Courses.Include("Topics").Include("Prowadzacy").Include("Files").FirstOrDefault(x=>x.Id == id);
            var coursesJson = Mapper.Map<Course, CoursesJson>(course);
            return coursesJson;
        }

        public void SaveCourse(CoursesJson course)
        {
            var courseDb = Mapper.Map<CoursesJson, Course>(course);
            _db.Courses.Add(courseDb);
            _db.SaveChanges();
        }

        public void SaveCourse(int courseId, CoursesJson course)
        {
            var courseDb = _db.Courses.FirstOrDefault(x => x.Id == courseId);
            courseDb.Name = course.Name;
            courseDb.Description = course.Description;
            _db.SaveChanges();
        }

        public void DeleteCourse(int courseId)
        {
            var courseDb = _db.Courses.FirstOrDefault(x => x.Id == courseId);
            _db.Courses.Remove(courseDb);
            _db.SaveChanges();
        }

        public void CloseCourse(int courseId)
        {
            var courseDb = _db.Courses.FirstOrDefault(x => x.Id == courseId);
            courseDb.Status = (int) eWarsztatyEnums.CourseStatus.Closed;
            _db.SaveChanges();
        }
    }
}