using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWarsztaty.Domain
{
    public class UzytkownikRola
    {
        [Key]
        public int UzytkownikRolaId { get; set; }

        [ForeignKey("RolaId")]
        //pozniej moze zmienie na Prowadzacy
        public virtual Rola Rola { get; set; }
        public int RolaId { get; set; }

        [ForeignKey("UzytkownikId")]
        //pozniej moze zmienie na Prowadzacy
        public virtual Uzytkownik Uzytkownik { get; set; }
        public int UzytkownikId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<UzytkownikRola>
        {
            public Map()
            {

                // blokowanie kasowania sprzedazzakup jesli ma powiązane warsztaty
                this.HasRequired(t => t.Rola)
                    .WithMany(t => t.UzytkownicyRole)
                    .HasForeignKey(d => d.RolaId)
                    .WillCascadeOnDelete(false);

                // blokowanie kasowania sprzedazzakup jesli ma powiązane warsztaty
                this.HasRequired(t => t.Uzytkownik)
                    .WithMany(t => t.UzytkownicyRole)
                    .HasForeignKey(d => d.UzytkownikId)
                    .WillCascadeOnDelete(false);
            }
        }
        #endregion
    }
}
