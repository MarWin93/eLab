using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWarsztaty.Domain
{
    public class UdzialWWarsztacie
    {
        public int UdzialWWarsztacieId { get; set; }
        public string Ocena { get; set; }
        public string KomentarzProwadzacego { get; set; }

        public bool Aktywny { get; set; }
        public bool AktywnyAgent { get; set; }
        public string AgentConnectionId { get; set; }

        [ForeignKey("UzytkownikId")]
        //pozniej moze zmienie na Prowadzacy
        public virtual Uzytkownik Uzytkownik { get; set; }
        public int UzytkownikId { get; set; }

        [ForeignKey("WarsztatId")]
        //pozniej moze zmienie na Prowadzacy
        public virtual Warsztat Warsztat { get; set; }
        public int WarsztatId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<UdzialWWarsztacie>
        {
            public Map()
            {

                // blokowanie kasowania sprzedazzakup jesli ma powiązane warsztaty
                this.HasRequired(t => t.Uzytkownik)
                    .WithMany(t => t.UdzialyWWarsztacie)
                    .HasForeignKey(d => d.UzytkownikId)
                    .WillCascadeOnDelete(false);

                // blokowanie kasowania sprzedazzakup jesli ma powiązane warsztaty
                this.HasRequired(t => t.Warsztat)
                    .WithMany(t => t.UdzialyWWarsztacie)
                    .HasForeignKey(d => d.WarsztatId)
                    .WillCascadeOnDelete(false);
            }
        }
        #endregion
    }
}