using System.Collections.Generic;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class TopicsJson
    {
        public TopicsJson()
        {
            this.Files = new List<FilesJson>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public bool IsArchived { get; set; }
        public ICollection<FilesJson> Files { get; set; }
    }
}