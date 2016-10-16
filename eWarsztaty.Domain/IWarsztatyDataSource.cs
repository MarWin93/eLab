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

        //elabs entites
        IQueryable<Class> Classes { get; }
        IQueryable<Course> Courses { get; }
        IQueryable<Group> Groups { get; }
        IQueryable<Topic> Topics { get; }
        IQueryable<ParticipationInCourse> Participations { get; }
        IQueryable<EnrollmentInTopic> Enrollments { get; }

        void Save();
    }
}
