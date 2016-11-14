using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class ClassJson
    {
        public ClassJson()
        {
            this.Groups = new List<GroupsJson>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
        public bool Closed { get; set; }

        public ICollection<GroupsJson> Groups { get; set; }
    }
}