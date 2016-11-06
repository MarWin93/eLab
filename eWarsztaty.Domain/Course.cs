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
        public string EnrollmentKey { get; set; }

        public ICollection<Topic> Topics { get; set; }
        public ICollection<Plik> Files { get; set; }

        public ICollection<ParticipationInCourse> Participations { get; set; }


        [ForeignKey("TeacherId")]
        public virtual Uzytkownik Teacher { get; set; }
        public int TeacherId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<Course>
        {
            public Map() {
                this.HasRequired(t => t.Teacher)
                .WithMany(t => t.Courses)
                       .HasForeignKey(d => d.TeacherId)
                       .WillCascadeOnDelete(false);
            }
        }
        #endregion
    }
}
