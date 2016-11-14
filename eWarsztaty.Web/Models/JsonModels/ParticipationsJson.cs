using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class ParticipationsJson
    {

       
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime ParticipationSince { get; set; }
        public DateTime ParticipationTo { get; set; }

    }
}