using System;
using System.Collections.Generic;
using eWarsztaty.Domain;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class CoursesJson
    {
        public CoursesJson()
        {
//          this.Topics = new List<Topic>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

//        public ICollection<Topic> Topics { get; set; }
        public int ProwadzacyId { get; set; }
    }
}