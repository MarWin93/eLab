﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class CoursesJson
    {
        public CoursesJson()
        {
            this.Topics = new List<TopicsJson>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Closed { get; set; }

        public ICollection<TopicsJson> Topics { get; set; }
    }
}