using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Models.ViewModels
{
    public class UczestnicyListView
    {
        public int UczestnikId { get; set; }
        public string Login { get; set; }
        public bool AgentAktywny { get; set; }
    }
}