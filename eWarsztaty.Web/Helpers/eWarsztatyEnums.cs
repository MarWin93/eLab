﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWarsztaty.Web.Helpers
{
    public class eWarsztatyEnums
    {
        public enum StatusWarsztatu
        {
            Zamkniety = 0,
            WTrakcie = 1,
            Zakonczony = 2,
        }

        public enum PrezentacjaZmianaStrony
        {

            Nastepna = 0,
            Poprzednia = 1,
        }
    }
}