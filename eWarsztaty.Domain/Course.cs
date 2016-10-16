using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace eWarsztaty.Domain
{
    public class Course
    {
        public Course()
        {
            this.Topics = new List<Topic>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public ICollection<Topic> Topics { get; set; }

        public ICollection<ParticipationInCourse> Participations { get; set; }


        //[ForeignKey("ProwadzacyId")]
        //public virtual Uzytkownik Prowadzacy { get; set; }
        //public int ProwadzacyId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<Course>
        {

        }
        #endregion
    }
}
