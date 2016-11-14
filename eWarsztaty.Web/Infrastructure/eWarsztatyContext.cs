using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using eWarsztaty.Domain;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace eWarsztaty.Web.Infrastructure
{
    public class eWarsztatyContext : DbContext, IWarsztatyDataSource
    {
        public DbSet<Warsztat> Warsztaty { get; set; }
        public DbSet<UdzialWWarsztacie> UdzialyWWarsztacie { get; set; }
        public DbSet<Uprawnienie> Uprawnienia { get; set; }
        public DbSet<UzytkownikRola> UzytkownicyRole { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Rola> Role { get; set; }
        public DbSet<UprawnienieRola> UprawnieniaRole { get; set; }
        public DbSet<Plik> Pliki { get; set; }

        //eLab Entity classes
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ParticipationInCourse> Participations { get; set; }
        public DbSet<EnrollmentInTopic> EnrolmentsInTopics { get; set; }



        public eWarsztatyContext():base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new UdzialWWarsztacie.Map());
            modelBuilder.Configurations.Add(new Warsztat.Map());
            modelBuilder.Configurations.Add(new UprawnienieRola.Map());
            modelBuilder.Configurations.Add(new UzytkownikRola.Map());
            modelBuilder.Configurations.Add(new Plik.Map());

            //elab maping methods added to context configuration
            modelBuilder.Configurations.Add(new Class.Map());
            modelBuilder.Configurations.Add(new Course.Map());
            modelBuilder.Configurations.Add(new Group.Map());
            modelBuilder.Configurations.Add(new Topic.Map());
        }

        //METODY NA BAZIE
        void IWarsztatyDataSource.Save()
        {
            SaveChanges();
        }

        //USER
        public void AddUser(Uzytkownik user)
        {
            Uzytkownicy.Add(user);
            SaveChanges();
        }

        public Uzytkownik GetUser(string userName)
        {
            var user = Uzytkownicy.FirstOrDefault(u => u.Login== userName);
            return user;
        }

        public Uzytkownik GetUser(string userName, string password)
        {
            var encryptedPassword = CustomMembershipProvider.GetMd5Hash(password);
            var user = Uzytkownicy.FirstOrDefault(u => u.Login ==
                           userName && u.Haslo == encryptedPassword);
            return user;
        }

        public void AddUserRole(UzytkownikRola userRole)
        {
            var roleEntry = UzytkownicyRole.FirstOrDefault(r => r.UzytkownikId == userRole.UzytkownikId);
            if (roleEntry != null)
            {
                UzytkownicyRole.Remove(roleEntry);
                SaveChanges();
            }
            UzytkownicyRole.Add(userRole);
            SaveChanges();
        }

        public void AddUserRole(int userId, int roleId)
        {
            var roleEntry = UzytkownicyRole.FirstOrDefault(r => r.UzytkownikId == userId && r.RolaId == roleId);
            if (roleEntry != null)
            {
                UzytkownicyRole.Remove(roleEntry);
                SaveChanges();
            }
            UzytkownikRola userRole = new UzytkownikRola() { RolaId = roleId, UzytkownikId = userId, Rola = Role.FirstOrDefault(r => r.RolaId == roleId) };
            var uzytkownik = Uzytkownicy.FirstOrDefault(x => x.UzytkownikId == userId);
            uzytkownik.UzytkownicyRole.Add(userRole);
            UzytkownicyRole.Add(userRole);
            SaveChanges();
        }

        public void AddUprawnieniaRole(List<int> listaId, string roleName)
        {
            var rola = Role.FirstOrDefault(x=>x.Nazwa == roleName);
            if (rola != null)
            {
                foreach (var uprawnienieId in listaId)
                {
                    var uprawnienie = Uprawnienia.FirstOrDefault(x => x.UprawnienieId == uprawnienieId);
                    if (uprawnienie != null)
                    {
                        UprawnienieRola ur = new UprawnienieRola() { RolaId = rola.RolaId, UprawnienieId = uprawnienie.UprawnienieId, Uprawnienie = uprawnienie, Rola = rola };
                        UprawnieniaRole.Add(ur);
                        uprawnienie.UprawnieniaRole.Add(ur);
                    }
                }
                SaveChanges();
            }
        }

        public void AddRolesUzytkownik(List<int> listaId, string uzytkownikName)
        {
            var uzytkownik = Uzytkownicy.FirstOrDefault(x => x.Login == uzytkownikName);
            if (uzytkownik != null)
            {
                foreach (var roleId in listaId)
                {
                    var rola = Role.FirstOrDefault(x => x.RolaId == roleId);
                    if (rola != null)
                    {
                        UzytkownikRola ur = new UzytkownikRola() { RolaId = roleId, UzytkownikId = uzytkownik.UzytkownikId, Rola = rola };
                        UzytkownicyRole.Add(ur);
                        uzytkownik.UzytkownicyRole.Add(ur);
                    }
                }
                SaveChanges();
            }
        }

        public void DeleteRole(string roleName)
        {
            var rola = Role.FirstOrDefault(x => x.Nazwa == roleName);
            if (rola != null)
            {
                Role.Remove(rola);
            }
            SaveChanges();
        }

        public void DeleteUzytkownik(string uzytkownikName)
        {
            var uzyt = Uzytkownicy.FirstOrDefault(x => x.Login == uzytkownikName);
            if (uzyt != null)
            {
                Uzytkownicy.Remove(uzyt);
            }
            SaveChanges();
        }

        public void RemoveAllUprawnieniaFromRole(string roleName)
        {
            var rola = Role.FirstOrDefault(x => x.Nazwa == roleName);
            if (rola != null)
            {
                var uprawnieniaRole = UprawnieniaRole.Where(x => x.RolaId == rola.RolaId).ToList();
                foreach (var ur in uprawnieniaRole)
                {
                    UprawnieniaRole.Remove(ur);
                }
                SaveChanges();
            }
        }

        public void RemoveAllRolesFromUzytkownik(string UzytkownikName)
        {
            var uzytkownik = Uzytkownicy.FirstOrDefault(x => x.Login == UzytkownikName);
            if (uzytkownik != null)
            {
                var roleUzytkownicy = UzytkownicyRole.Where(x => x.UzytkownikId == uzytkownik.UzytkownikId).ToList();
                foreach (var ur in roleUzytkownicy)
                {
                    UzytkownicyRole.Remove(ur);
                }
                SaveChanges();
            }
        }

        public int GetIdUprawnienia(string uprawnienieName)
        {
            int idUprawnienia = Uprawnienia.FirstOrDefault(x => x.Nazwa == uprawnienieName).UprawnienieId;
            return idUprawnienia; 

        }
        //ROLE
        public void AddRole(string roleName, string roleDescription = "")
        {
            Rola rola = new Rola() { Nazwa = roleName, Opis = roleDescription };
            Role.Add(rola);
            SaveChanges();
        }

        public Rola GetRola(string roleName)
        {
            var role = Role.FirstOrDefault(u => u.Nazwa == roleName);
            return role;
        }

        public Rola GetRola(int roleId)
        {
            var role = Role.FirstOrDefault(u => u.RolaId == roleId);
            return role;
        }

        //GET
        //IQueryable<Uczestnik> IWarsztatyDataSource.Uczestnicy
        //{
        //    get { return Uczestnicy; }
        //}

        IQueryable<Warsztat> IWarsztatyDataSource.Warsztaty
        {
            get { return Warsztaty; }
        }

        IQueryable<UdzialWWarsztacie> IWarsztatyDataSource.UdzialyWWarsztacie
        {
            get { return UdzialyWWarsztacie; }
        }

        IQueryable<Uprawnienie> IWarsztatyDataSource.Uprawnienia
        {
            get { return Uprawnienia; }
        }

        IQueryable<UprawnienieRola> IWarsztatyDataSource.UprawnieniaRole
        {
            get { return UprawnieniaRole; }
        }

        IQueryable<Uzytkownik> IWarsztatyDataSource.Uzytkownicy
        {
            get { return Uzytkownicy; }
        }

        IQueryable<Rola> IWarsztatyDataSource.Role
        {
            get { return Role; }
        }

        IQueryable<UzytkownikRola> IWarsztatyDataSource.UzytkownicyRole
        {
            get { return UzytkownicyRole; }
        }

        IQueryable<Class> IWarsztatyDataSource.Classes
        {
            get { return Classes; }
        }

        IQueryable<Course> IWarsztatyDataSource.Courses
        {
            get { return Courses; }
        }

        IQueryable<Group> IWarsztatyDataSource.Groups
        {
            get { return Groups; }
        }

        IQueryable<Topic> IWarsztatyDataSource.Topics
        {
            get { return Topics; }
        }


        IQueryable<ParticipationInCourse> IWarsztatyDataSource.Participations
        {
            get { return Participations; }
        }

        IQueryable<EnrollmentInTopic> IWarsztatyDataSource.Enrollments
        {
            get { return EnrolmentsInTopics; }
        }
    }
}