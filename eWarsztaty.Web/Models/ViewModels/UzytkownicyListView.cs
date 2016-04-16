using eWarsztaty.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class UzytkownicyListView
    {
        public int UzytkownikId { get; set; }
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Display(Name="Haslo")]
        public string Haslo { get; set; }
        [Display(Name="Adres Email")]
        public string AdresEmail { get; set; }

        public List<Rola> Role { get; set; }

        public UzytkownicyListView()
        {
            Role = new List<Rola>();
        }
    }
}