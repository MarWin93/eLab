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

        public TopicsJson GetTopicById(int id)
        {
            var topic = _db.Topics.Include("Course").Include("Classes").FirstOrDefault(x => x.Id == id);
            var topicJson = Mapper.Map<Topic, TopicsJson>(topic);
            return topicJson;
        }

        public void SaveTopic(TopicsJson topic)
        {
            var topicDb = Mapper.Map<TopicsJson, Topic>(topic);
            _db.Topics.Add(topicDb);
            _db.SaveChanges();
        }

        public void SaveTopic(int topicId, TopicsJson group)
        {
            var topicDb = _db.Topics.FirstOrDefault(x => x.Id == topicId);
            topicDb.Name = group.Name;
            _db.SaveChanges();
        }

        public void DeleteTopic(int topicId)
        {
            var topicDb = _db.Topics.FirstOrDefault(x => x.Id == topicId);
            _db.Topics.Remove(topicDb);
            _db.SaveChanges();
        }
    }
}