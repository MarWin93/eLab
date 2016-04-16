using eWarsztaty.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class RoleView
    {
        public int RolaId { get; set; }
        public List<Uprawnienie> Uprawnienia { get; set; }
        public int[] SelectedUprawnienia { get; set; }
        [Display(Name="Nazwa")]
        [Required]
        public string Nazwa { get; set; }
        [Display(Name = "Opis")]
        public string Opis { get; set; }
        public RoleView()
        {
            this.Uprawnienia = new List<Uprawnienie>();
        }
    }
}