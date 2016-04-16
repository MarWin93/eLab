using eWarsztaty.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class ZapisanieView
    {

        public int SelectedWarsztatId { get; set; }
        public IEnumerable<SelectListItem> WszystkieWarsztaty{ get; set; }

        [Required]
        [Display(Name = "Hasło Dostępu")]
        public string HasloDostepu { get; set; }
    }
}