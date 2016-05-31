using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}