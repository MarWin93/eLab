using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class AddRoleToUserView
    {
        public int SelectedUserId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Uzytkownicy { get; set; }
        [Required]
        [Display(Name="Nazwa użytkownika")]
        public string UserName{get;set;}
        [Required]
        [Display(Name = "Nazwa roli")]
        public string RoleName { get; set; }
        public int SelectedRoleId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Role { get; set; }
    }
}