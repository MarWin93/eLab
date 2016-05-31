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
        public string Get(int id)
        {
            return "value";
        }

        // POST api/topics
        public void Post([FromBody]string value)
        {
        }

        // PUT api/topics/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/topics/5
        public void Delete(int id)
        {
        }
    }
}
