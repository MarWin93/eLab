using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class EnrollmentInTopicJson
    {

        public string ConnectionId { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }

        public string UserName { get; set; }

    }
}