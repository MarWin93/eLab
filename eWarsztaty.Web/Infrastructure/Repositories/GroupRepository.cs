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
    }
}