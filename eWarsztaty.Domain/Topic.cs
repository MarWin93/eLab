using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace eWarsztaty.Domain
{
    public class Topic
    {
        public Topic()
        {
            this.Classes = new List<Class>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public ICollection<Class> Classes { get; set; }
        public ICollection<Plik> Files { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        public int? CourseId { get; set; }

      //  public string EnrollmentKey { get; set; }

        public ICollection<EnrollmentInTopic> EnrollmentsInTopics { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<Topic>
        {
            public Map()
            {
                this.HasOptional(t => t.Course)
                    .WithMany(t => t.Topics)
                    .HasForeignKey(d => d.CourseId)
                    .WillCascadeOnDelete(true);
            }
        }
        #endregion
    }
}
