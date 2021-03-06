﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWarsztaty.Domain
{
    public class EnrollmentInTopic
    {


        [Key]
        public int Id { get; set; }

        public bool Active { get; set; }

        public string ConnectionId { get; set; }

        // Zapisany od tego czasu - uzupełniane w momencie zapisania się na warsztaty (topic)
        //public DateTime? EnrollmentSince { get; set; }

        // Zapisany do tego czasu - uzupełniane w momencie wypisania się z warsztatów (topic)
       // public DateTime? EnrollmentTo { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        public int UserId { get; set; }

        public string UserName { get; set; }

        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
        public int TopicId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<EnrollmentInTopic>
        {
            public Map()
            {
                this.HasRequired(t => t.Topic)
                    .WithMany(t => t.EnrollmentsInTopics)
                    .HasForeignKey(d => d.TopicId)
                    .WillCascadeOnDelete(false);

                this.HasRequired(t => t.User)
                   .WithMany(t => t.EnrollmentsInTopics)
                   .HasForeignKey(d => d.UserId)
                   .WillCascadeOnDelete(false);
            }
        }
        #endregion

    }
}
