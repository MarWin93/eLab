using System.Collections.Generic;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class GroupsJson
    {
        public GroupsJson()
        {
            this.Students = new List<StudentJson>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<StudentJson> Students { get; set; }
    }
}