using eWarsztaty.Web.Infrastructure.Repositories;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eWarsztaty.Web.Controllers
{
    public class TopicsController : ApiController
    {
        // GET api/topics
        public IEnumerable<TopicsJson> Get()
        {
            var topicRepository = new TopicRepository();
            return topicRepository.GetAllTopics();
        }

        // GET api/topics/5
        public TopicsJson Get(int id)
        {
            if (id > 0)
            {

                var topicRepository = new TopicRepository();
                return topicRepository.GetTopicById(id);
            }
            else
            {
                var newTopic = new TopicsJson();
                return newTopic;
            }
        }

        // POST api/topics
        public IHttpActionResult Post([FromBody]TopicsJson topic)
        {
            var topicRepository = new TopicRepository();
            topicRepository.SaveTopic(topic);
            return Ok();
        }

        // PUT api/topics/5
        public IHttpActionResult Put(int id, [FromBody]TopicsJson topic)
        {
            var topicRepository = new TopicRepository();
            topicRepository.SaveTopic(id, topic);
            return Ok();
        }

        // DELETE api/topics/5
        public void Delete(int id)
        {
            var topicRepository = new TopicRepository();
            topicRepository.DeleteTopic(id);
        }
    }
}
