using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace eWarsztaty.Domain
{
    public partial class ChatMessageDetail
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Uzytkownik User { get; set; }

        public int TopicId { get; set; }
        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
    }


    #region Map
    public class Map : EntityTypeConfiguration<ChatMessageDetail>
    {
        public Map()
        {

            this.HasRequired(t => t.Topic)
                    .WithMany(t => t.ChatMessageDetails)
                    .HasForeignKey(d => d.TopicId)
                    .WillCascadeOnDelete(false);

            this.HasRequired(t => t.User)
               .WithMany(t => t.ChatMessageDetails)
               .HasForeignKey(d => d.UserId)
               .WillCascadeOnDelete(false);
        }
    }
    #endregion
}
