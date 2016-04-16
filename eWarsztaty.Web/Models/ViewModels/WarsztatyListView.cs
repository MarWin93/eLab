using eWarsztaty.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class WarsztatyListView
    {
        public int WarsztatId { get; set; }
        public int ProwadzacyId { get; set; }
        [Display(Name = "Nazwa warsztatu")]
        [Required]
        public string Nazwa { get; set; }
        [Display(Name = "Data rozpoczęcia")]
        [DataType(DataType.DateTime)]
        [Required]
        public  DateTime? DataRozpoczecia { get; set; }
        [Required]
        [Display(Name = "Temat warsztatu")]
        public string Temat { get; set; }
        [Required]
        [Display(Name = "Hasło dostępu")]
        public string HasloDostepu { get; set; }
        [Display(Name = "Czas trwania")]
        public string CzasTrwania { get; set; } 

        [Display(Name = "Statu")]
        public int StatusWarsztatu { get; set; }

        //public List<Warsztat> Warsztaty{ get; set; }
        
    }
}