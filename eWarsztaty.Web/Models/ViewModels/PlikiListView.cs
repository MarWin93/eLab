using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class PlikiListView
    {
        public int PlikId { get; set; }
        public int ProwadzacyId { get; set; }
        public string Nazwa { get; set; }
        public string Rozszerzenie { get; set; }
    }
}