using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class GroupRepository
    {
        private eWarsztatyContext _db = new eWarsztatyContext();

        public IEnumerable<GroupsJson> GetAllGroups()
        {
            var groups = _db.Groups.ToList();
            var groupsJson = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupsJson>>(groups);
            return groupsJson;
        }

        public GroupsJson GetGroupById(int id)
        {
            var course = _db.Groups.Include("Topics").Include("Prowadzacy").FirstOrDefault(x => x.Id == id);
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
    }
}