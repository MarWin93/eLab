using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class EnrollmentInTopicJson
    {

     
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime EnrollmentSince { get; set; }
        public DateTime EnrollmentTo { get; set; }
        
    }
}