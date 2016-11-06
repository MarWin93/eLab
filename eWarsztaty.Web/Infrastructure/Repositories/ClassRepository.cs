using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eWarsztaty.Web.Helpers;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class ClassRepository
    {
        private eWarsztatyContext _db = new eWarsztatyContext();

        public IEnumerable<ClassJson> GetAllClasses()
        {
            var classes = _db.Classes.Include("Groups").ToList();
            var classesJson = Mapper.Map<IEnumerable<Class>, IEnumerable<ClassJson>>(classes);
            return classesJson;
        }

        public ClassJson GetClassById(int id)
        {
            var classDb = _db.Classes.Include("Groups").FirstOrDefault(x => x.Id == id);
            var classJson = Mapper.Map<Class, ClassJson>(classDb);
            return classJson;
        }

        public void SaveClass(ClassJson classJson)
        {
            var classDb = Mapper.Map<ClassJson, Class>(classJson);
            _db.Classes.Add(classDb);
            _db.SaveChanges();
        }

        public void SaveClass(int classId, ClassJson classJson)
        {
            var classDb = _db.Classes.FirstOrDefault(x => x.Id == classId);
            classDb.Name = classJson.Name;
            classDb.Description = classJson.Description;
            _db.SaveChanges();
        }

        public void DeleteClass(int classId)
        {
            var classDb = _db.Classes.FirstOrDefault(x => x.Id == classId);
            _db.Classes.Remove(classDb);
            _db.SaveChanges();
        }

        public void CloseClass(int classId)
        {
            var classDb = _db.Classes.FirstOrDefault(x => x.Id == classId);
            classDb.Status = (int)eWarsztatyEnums.TopicStatus.Closed;
            _db.SaveChanges();
        }
    }
}