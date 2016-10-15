using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;


namespace eWarsztaty.Domain
{
    public class Participation
    {

        [Key]
        public int Id { get; set; }

        public bool Active { get; set; }

        // uczestniczy od teggo czasu - uzupełniane w momencie dołączania zapisania się do kursu
        public DateTime? ParticipationSince { get; set; }

        // uczestniczy do tego czasu - uzupełniane w momencie wypisania się z kursu
        public DateTime? ParticipationTo { get; set; }

        [ForeignKey("UserId")]
        public virtual Uzytkownik User { get; set; }
        public int UserId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        public int CourseId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<Participation>
        {
            public Map()
            {
                this.HasRequired(t => t.Course)
                    .WithMany(t => t.Participations)
                    .HasForeignKey(d => d.CourseId)
                    .WillCascadeOnDelete(false);

                this.HasRequired(t => t.User)
                   .WithMany(t => t.Participations)
                   .HasForeignKey(d => d.UserId)
                   .WillCascadeOnDelete(false);
            }
        }
        #endregion
    }
}
