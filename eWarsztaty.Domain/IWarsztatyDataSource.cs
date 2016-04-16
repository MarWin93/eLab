using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWarsztaty.Domain
{
    public interface IWarsztatyDataSource
    {
        IQueryable<Warsztat> Warsztaty { get; }
        IQueryable<UdzialWWarsztacie> UdzialyWWarsztacie { get; }
        IQueryable<Uprawnienie> Uprawnienia { get; }
        IQueryable<UprawnienieRola> UprawnieniaRole { get; }
        IQueryable<Uzytkownik> Uzytkownicy { get; }
        IQueryable<Rola> Role { get; }
        IQueryable<UzytkownikRola> UzytkownicyRole { get; }


        void Save();
    }
}
