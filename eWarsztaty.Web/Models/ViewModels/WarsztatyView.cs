using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class WarsztatyView
    {
        public int WarsztatId { get; set; }
        public int ProwadzacyId { get; set; }
        public string Nazwa { get; set; }
        public string Temat { get; set; }
    }
}