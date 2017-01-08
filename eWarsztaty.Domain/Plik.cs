using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace eWarsztaty.Domain
{
    public class Plik
    {
        public int PlikId { get; set; }
        public string Nazwa { get; set; }
        public string Rozszerzenie { get; set; }
        public string Size { get; set; } //in kb
        public bool Zadanie { get; set; } //depricated

        public byte[] File { get; set; }


        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        public int? CourseId { get; set; }

        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
        public int? TopicId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<Plik>
        {
            public Map()
            {
                HasOptional(t => t.Course)
                    .WithMany(t => t.Files)
                    .HasForeignKey(d => d.CourseId)
                    .WillCascadeOnDelete(true);

                HasOptional(t => t.Topic)
                    .WithMany(t => t.Files)
                    .HasForeignKey(d => d.TopicId)
                    .WillCascadeOnDelete(false);
            }
        }
        #endregion
    }
}
