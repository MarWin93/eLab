using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class RoleListView
    {
        public int RolaId { get; set; }
        [Display(Name="Nazwa")]
        public string Nazwa { get; set; }
        [Display(Name = "Opis")]
        public string Opis { get; set; }

    }
}