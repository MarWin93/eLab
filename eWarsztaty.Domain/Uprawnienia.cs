using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eWarsztaty.Domain
{
    public class Uprawnienie
    {
        public Uprawnienie()
        {
            this.UprawnieniaRole = new List<UprawnienieRola>();
        }

        [Key]
        public int UprawnienieId { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }
        public ICollection<UprawnienieRola> UprawnieniaRole { get; set; }
    }
}
