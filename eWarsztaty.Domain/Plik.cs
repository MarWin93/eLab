using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWarsztaty.Domain
{
    public class Plik
    {
        public int PlikId { get; set; }
        public string Nazwa { get; set; }
        public string Rozszerzenie { get; set; }
        public bool Zadanie { get; set; }

        public byte[] File { get; set; }

        [ForeignKey("WarsztatId")]
        public virtual Warsztat Warsztat { get; set; }
        public int WarsztatId { get; set; }

        #region Map
        public class Map : EntityTypeConfiguration<Plik>
        {
            public Map()
            {
                this.HasRequired(t => t.Warsztat)
                    .WithMany(t => t.Pliki)
                    .HasForeignKey(d => d.WarsztatId)
                    .WillCascadeOnDelete(false);
            }
        }
        #endregion
    }
}
