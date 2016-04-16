using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWarsztaty.Domain
{
    public class Rola
    {
        public Rola()
        {
            this.UprawnieniaRole = new List<UprawnienieRola>();
            this.UzytkownicyRole = new List<UzytkownikRola>();
        }

        [Key]
        public int RolaId { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(450)]
        public string Nazwa { get; set; }
        public string Opis { get; set; }
        public ICollection<UprawnienieRola> UprawnieniaRole { get; set; }
        public ICollection<UzytkownikRola> UzytkownicyRole { get; set; }
    }
}
