using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class MojeKursyView 
    {
        [HiddenInput(DisplayValue = false)]
        public int IdUczestnika { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int IdWarsztatu { get; set; }

        [Display(Name= "Data i czas" )]
        public DateTime? DataCzas { get; set; }

        [Display(Name = "Liczba gogdzin")]
        public string LiczbaGodzin { get; set; }

        [Display(Name = "Ocena")]
        public string Ocena { get; set; }

        [Display(Name = "Temat")]
        public string Temat { get; set; }
       


    }
}