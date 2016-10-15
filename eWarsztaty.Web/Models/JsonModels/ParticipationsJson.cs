using System.Collections.Generic;
using Newtonsoft.Json;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class ParticipationsJson
    {
       
        public ParticipationsJson()
        {
            this.Students = new List<StudentJson>();
        }

        public int Id { get; set; }

        public ICollection<StudentJson> Students { get; set; }
    }
}