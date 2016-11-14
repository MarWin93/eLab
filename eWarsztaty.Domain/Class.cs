using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace eWarsztaty.Domain
{
    public class Class
    {
        public Class()
        {
            this.Groups = new List<Group>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public ICollection<Group> Groups { get; set; }

        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
        public int TopicId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<Class>
        {
            public Map()
            {
                this.HasRequired(t => t.Topic)
                    .WithMany(t => t.Classes)
                    .HasForeignKey(d => d.TopicId)
                    .WillCascadeOnDelete(true);
            }
        }
        #endregion
    }
}
