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
            var groups = _db.Groups.Include("Students").ToList();
            var groupsJson = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupsJson>>(groups);
            return groupsJson;
        }

        public GroupsJson GetGroupById(int id)
        {
            var group = _db.Groups.Include("Topics").Include("Prowadzacy").FirstOrDefault(x => x.Id == id);
            var groupJson = Mapper.Map<Group, GroupsJson>(group);
            return groupJson;
        }

        public void SaveGroup(GroupsJson group)
        {
            var groupDb = Mapper.Map<GroupsJson, Group>(group);
            _db.Groups.Add(groupDb);
            _db.SaveChanges();
        }

        public void SaveGroup(int groupId, GroupsJson group)
        {
            var groupDb = _db.Groups.FirstOrDefault(x => x.Id == groupId);
            groupDb.Name = group.Name;
            _db.SaveChanges();
        }

        public void DeleteGroup(int groupId)
        {
            var groupDb = _db.Groups.FirstOrDefault(x => x.Id == groupId);
            _db.Groups.Remove(groupDb);
            _db.SaveChanges();
        }
    }
}