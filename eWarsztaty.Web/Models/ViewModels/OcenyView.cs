using eWarsztaty.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class OcenyView
    {
        public int WarsztatId { get; set; }
        public string[] Oceny{ get; set; }
        public List<Uzytkownik> Uzytkownicy { get; set; }

        public OcenyView()
        {
            Uzytkownicy = new List<Uzytkownik>();
        }
    }
}