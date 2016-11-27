using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class MesssageJson
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }

        public DateTime SendDateTime { get; set; }

    }
}