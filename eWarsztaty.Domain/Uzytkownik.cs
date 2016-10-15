using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eWarsztaty.Domain
{
    public class Uzytkownik
    {
        public Uzytkownik()
        {
            this.UdzialyWWarsztacie = new List<UdzialWWarsztacie>();
            this.UzytkownicyRole = new List<UzytkownikRola>();
            this.Warsztaty = new List<Warsztat>();
        }

        [Key]
        public int UzytkownikId { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(450)]
        public string Login { get; set; }
        [Required]
        public string Haslo{ get; set; }
        [Required]
        public string AdresEmail { get; set; }

        public ICollection<UzytkownikRola> UzytkownicyRole { get; set; }

        public ICollection<UdzialWWarsztacie> UdzialyWWarsztacie { get; set; }

        public ICollection<Warsztat> Warsztaty { get; set; }

    }
}
