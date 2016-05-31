using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class TopicRepository
    {
        private eWarsztatyContext _db = new eWarsztatyContext();

        public IEnumerable<TopicsJson> GetAllTopics()
        {
            var topics = _db.Topics.ToList();
            var topicsJson = Mapper.Map<IEnumerable<Topic>, IEnumerable<TopicsJson>>(topics);
            return topicsJson;
        }
    }
}