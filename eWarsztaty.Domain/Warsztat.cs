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
    public class Warsztat
    {
        public Warsztat()
        {
            this.UdzialyWWarsztacie = new List<UdzialWWarsztacie>();
            this.Pliki = new List<Plik>();
        }

        [Key]
        public int WarsztatId { get; set; }
        public string Nazwa { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataRozpoczecia { get; set; }
        public string Temat { get; set; }
        public string CzasTrwania { get; set; }
        public string HasloDostepu{ get; set; }

        public int StatusWarsztatu { get; set; }

        public ICollection<UdzialWWarsztacie> UdzialyWWarsztacie { get; set; }
        public ICollection<Plik> Pliki { get; set; }

        [ForeignKey("ProwadzacyId")]
        //pozniej moze zmienie na Prowadzacy
        public virtual Uzytkownik Prowadzacy { get; set; }
        public int ProwadzacyId { get; set; }

        public string ProwadzacyConnectionId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<Warsztat>
        {
            public Map()
            {

                // blokowanie kasowania sprzedazzakup jesli ma powiązane warsztaty
                this.HasRequired(t => t.Prowadzacy)
                    .WithMany(t => t.Warsztaty)
                    .HasForeignKey(d => d.ProwadzacyId)
                    .WillCascadeOnDelete(false);
            }
        }
        #endregion

    }
}
