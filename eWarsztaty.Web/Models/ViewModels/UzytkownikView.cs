using eWarsztaty.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class UzytkownikView
    {

        public int UzytkownikId { get; set; }

        [Display(Name = "Role użytkownika:")]
        public List<Rola> Role{ get; set; }
        public int[] SelectedRole { get; set; }
        

        [Display(Name = "Login")]
        [Required]
        public string Login { get; set; }
        [Display(Name = "Haslo")]
        [StringLength(100, ErrorMessage = "{0} musi składać się z przynajmniej {2} znaków.", MinimumLength = 6)]
        public string Haslo { get; set; }
        [Display(Name = "Adres Email")]
        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage="Niepoprawny format email")]
        public string AdresEmail { get; set; }
        public UzytkownikView()
        {
            this.Role = new List<Rola>();
        }

    }
}