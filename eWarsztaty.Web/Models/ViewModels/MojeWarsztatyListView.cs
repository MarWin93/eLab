using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class MojeWarsztatyListView
    {
        public int WarsztatId { get; set; }
        public int ProwadzacyId { get; set; }
        [Display(Name = "Nazwa warsztatu")]
        public string Nazwa { get; set; }
        [Display(Name = "Data rozpoczęcia")]
        [DataType(DataType.DateTime)]
        public DateTime? DataRozpoczecia { get; set; }
        [Display(Name = "Temat warsztatu")]
        public string Temat { get; set; }
        [Display(Name = "Czas trwania")]
        public string CzasTrwania { get; set; }

        [Display(Name = "Status")]
        public int StatusWarsztatu { get; set; }

    }
}