using eWarsztaty.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class CurrentRoleView
    {
        [Display(Name = "rola użytkownika")]
        public int SelectedUserRoleId { get; set; }
        public IEnumerable<SelectListItem> UserRoles { get; set; }
    }
}