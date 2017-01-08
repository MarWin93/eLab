using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace eWarsztaty.Domain
{
    public class Group
    {
        public Group()
        {
            this.Students = new List<User>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }        

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
        public int ClassId { get; set; }

        public ICollection<User> Students { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<Group>
        {
            public Map()
            {
                this.HasRequired(t => t.Class)
                    .WithMany(t => t.Groups)
                    .HasForeignKey(d => d.ClassId)
                    .WillCascadeOnDelete(true);
            }
        }
        #endregion
    }
}
