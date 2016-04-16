using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWarsztaty.Domain
{
    public class UprawnienieRola
    {
        public int UprawnienieRolaId { get; set; }

        [ForeignKey("RolaId")]
        public Rola Rola { get; set; }
        public int RolaId { get; set; }

        [ForeignKey("UprawnienieId")]
        //pozniej moze zmienie na Prowadzacy
        public virtual Uprawnienie Uprawnienie { get; set; }
        public int UprawnienieId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<UprawnienieRola>
        {
            public Map()
            {

                // blokowanie kasowania sprzedazzakup jesli ma powiązane Rola
                this.HasRequired(t => t.Rola)
                    .WithMany(t => t.UprawnieniaRole)
                    .HasForeignKey(d => d.RolaId)
                    .WillCascadeOnDelete(false);

                // blokowanie kasowania sprzedazzakup jesli ma powiązane Uprawnienie
                this.HasRequired(t => t.Uprawnienie)
                    .WithMany(t => t.UprawnieniaRole)
                    .HasForeignKey(d => d.UprawnienieId)
                    .WillCascadeOnDelete(false);
            }
        }
        #endregion
    }
}
