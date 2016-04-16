namespace eWarsztaty.Web.Infrastructure
{
    using eWarsztaty.Domain;
    using eWarsztaty.Web.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;

    internal sealed class Configuration : DbMigrationsConfiguration<eWarsztaty.Web.Infrastructure.eWarsztatyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }

        protected override void Seed(eWarsztaty.Web.Infrastructure.eWarsztatyContext context)
        {

            if (!Roles.RoleExists("Admin"))
            {
                Roles.CreateRole("Admin");
            }
            if (!Roles.RoleExists("Uczestnik"))
            {
                Roles.CreateRole("Uczestnik");
            }
            if (!Roles.RoleExists("Prowadzacy"))
            {
                Roles.CreateRole("Prowadzacy");
            }

            if (Membership.GetUser("smarcin", false) == null)
            {
                string haslo = CustomMembershipProvider.GetMd5Hash("smarcin");
                Uzytkownik smarcin = new Uzytkownik() { Login = "smarcin", Haslo = haslo, AdresEmail = "smarcin@wp.pl" };
                context.AddUser(smarcin);
                CustomRoleProvider.AddUserToRole("smarcin", "Admin");
            }
            if (Membership.GetUser("MarcinWinkler", false) == null)
            {
                string haslo = CustomMembershipProvider.GetMd5Hash("MarcinWinkler");
                Uzytkownik smarcin = new Uzytkownik() { Login = "MarcinWinkler", Haslo = haslo, AdresEmail = "MarcinWinkler@wp.pl" };
                context.AddUser(smarcin);
                CustomRoleProvider.AddUserToRole("MarcinWinkler", "Uczestnik");
            }
            if (Membership.GetUser("MariaKlima", false) == null)
            {
                string haslo = CustomMembershipProvider.GetMd5Hash("MariaKlima");
                Uzytkownik smarcin = new Uzytkownik() { Login = "MariaKlima", Haslo = haslo, AdresEmail = "MariaKlima@wp.pl" };
                context.AddUser(smarcin);
                CustomRoleProvider.AddUserToRole("MariaKlima", "Uczestnik");
            }
            if (Membership.GetUser("LukaszNowak", false) == null)
            {
                string haslo = CustomMembershipProvider.GetMd5Hash("LukaszNowak");
                Uzytkownik smarcin = new Uzytkownik() { Login = "LukaszNowak", Haslo = haslo, AdresEmail = "LukaszNowak@wp.pl" };
                context.AddUser(smarcin);
                CustomRoleProvider.AddUserToRole("LukaszNowak", "Prowadzacy");
            }
            if (Membership.GetUser("AnnaKowalska", false) == null)
            {
                string haslo = CustomMembershipProvider.GetMd5Hash("AnnaKowalska");
                Uzytkownik smarcin = new Uzytkownik() { Login = "AnnaKowalska", Haslo = haslo, AdresEmail = "AnnaKowalska@wp.pl" };
                context.AddUser(smarcin);
                CustomRoleProvider.AddUserToRole("AnnaKowalska", "Prowadzacy");
            }


            ////Uprawnienia do widokow
            context.Uprawnienia.AddOrUpdate(d => d.UprawnienieId,
                new Uprawnienie() { Nazwa = "Home-EditProfile", Opis = "edytuj sw�j profil" },
                new Uprawnienie() {Nazwa = "Role-Index", Opis = "poka� widok z rolami" },
                new Uprawnienie() { Nazwa = "Role-Save", Opis = "dodaj now� rol�" },
                new Uprawnienie() { Nazwa = "Role-Detail", Opis = "edytuj istniej�c� rol�" },
                new Uprawnienie() { Nazwa = "Role-Delete", Opis = "usu� istniej�c� rol�" },
                new Uprawnienie() { Nazwa = "Uzytkownicy-PrzypiszRole", Opis = "przypisz rol� u�ytkownikom" },
                new Uprawnienie() { Nazwa = "Uzytkownicy-Index", Opis = "poka� widok z u�ytkownikami" },
                new Uprawnienie() { Nazwa = "Uzytkownicy-Save", Opis = "dodaj nowego u�ytkownika" },
                new Uprawnienie() { Nazwa = "Uzytkownicy-Detail", Opis = "edytuj istniej�cego u�ytkownika" },
                new Uprawnienie() { Nazwa = "Uzytkownicy-Delete", Opis = "usu� istniej�cego u�ytkownika" },
                new Uprawnienie() { Nazwa = "Warsztaty-MojeWarsztaty", Opis = "poka� widok moich warsztat�w" },
                new Uprawnienie() { Nazwa = "Warsztaty-Zapis", Opis = "zapisz si� na warsztat" },
                new Uprawnienie() { Nazwa = "Warsztaty-Wypisanie", Opis = "wypisz si� z warsztatu" },
                new Uprawnienie() { Nazwa = "Warsztaty-Index", Opis = "poka� widok z wszystkimi warsztatami" },
                new Uprawnienie() { Nazwa = "Warsztaty-Save", Opis = "dodaj nowy warsztat" },
                new Uprawnienie() { Nazwa = "Warsztaty-Detail", Opis = "edytuj istniej�cy warsztat" },
                new Uprawnienie() { Nazwa = "Warsztaty-Delete", Opis = "usu� istniej�cy warsztat" }
                );
            context.SaveChanges();


            CustomRoleProvider.AddPrivilegesToRole(new List<int>(){ 
            context.GetIdUprawnienia("Warsztaty-MojeWarsztaty") , 
            context.GetIdUprawnienia("Warsztaty-Zapis") , 
            context.GetIdUprawnienia("Warsztaty-Wypisanie") , 
            context.GetIdUprawnienia("Warsztaty-Index") , 
            context.GetIdUprawnienia("Warsztaty-Save") , 
            context.GetIdUprawnienia("Warsztaty-Detail") , 
            context.GetIdUprawnienia("Warsztaty-Delete") , 
            context.GetIdUprawnienia("Role-Index") , 
            context.GetIdUprawnienia("Role-Save"),
            context.GetIdUprawnienia("Role-Detail"),
            context.GetIdUprawnienia("Role-Delete"),
            context.GetIdUprawnienia("Uzytkownicy-Index"),
            context.GetIdUprawnienia("Uzytkownicy-Save") ,
            context.GetIdUprawnienia("Uzytkownicy-Detail"),
            context.GetIdUprawnienia("Home-EditProfile"),
            context.GetIdUprawnienia("Uzytkownicy-PrzypiszRole"),
            context.GetIdUprawnienia("Uzytkownicy-Delete")}, "Admin");

            CustomRoleProvider.AddPrivilegesToRole(new List<int>(){ 
            context.GetIdUprawnienia("Warsztaty-MojeWarsztaty") , 
            context.GetIdUprawnienia("Warsztaty-Zapis") , 
            context.GetIdUprawnienia("Warsztaty-Wypisanie") ,   
            context.GetIdUprawnienia("Home-EditProfile")}, "Uczestnik");
            context.SaveChanges();

            CustomRoleProvider.AddPrivilegesToRole(new List<int>(){ 
            context.GetIdUprawnienia("Warsztaty-MojeWarsztaty") , 
            context.GetIdUprawnienia("Warsztaty-Zapis") , 
            context.GetIdUprawnienia("Warsztaty-Wypisanie") , 
            context.GetIdUprawnienia("Warsztaty-Index") , 
            context.GetIdUprawnienia("Warsztaty-Save") , 
            context.GetIdUprawnienia("Warsztaty-Detail") , 
            context.GetIdUprawnienia("Warsztaty-Delete")}, "Prowadzacy");


            context.SaveChanges();

            context.Warsztaty.AddOrUpdate(d => d.Temat, new Warsztat() { Nazwa = "BSK", CzasTrwania = "2h", DataRozpoczecia = new DateTime(2015, 1, 10), Temat = "Kontrola dost�pu", ProwadzacyId = 1, HasloDostepu = "098f6bcd4621d373cade4e832627b4f6",  StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety },
            new Warsztat() { Nazwa = "BSK", CzasTrwania = "2h", DataRozpoczecia = new DateTime(2015, 2, 10), Temat = "Polityka Prywatno�ci", ProwadzacyId = 1, HasloDostepu = "098f6bcd4621d373cade4e832627b4f6", StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety },
            new Warsztat() { Nazwa = "BSK", CzasTrwania = "3h", DataRozpoczecia = new DateTime(2015, 3, 10), Temat = "Zapory Ogniowe", ProwadzacyId = 1, HasloDostepu = "098f6bcd4621d373cade4e832627b4f6", StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety },
            new Warsztat() { Nazwa = "KSR", CzasTrwania = "1h", DataRozpoczecia = new DateTime(2015, 1, 15), Temat = "WCF", ProwadzacyId = 1, HasloDostepu = "098f6bcd4621d373cade4e832627b4f6", StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety },
            new Warsztat() { Nazwa = "KSR", CzasTrwania = "2h", DataRozpoczecia = new DateTime(2015, 2, 15), Temat = "MSMQ", ProwadzacyId = 1, HasloDostepu = "098f6bcd4621d373cade4e832627b4f6", StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety },
            new Warsztat() { Nazwa = "KSR", CzasTrwania = "3h", DataRozpoczecia = new DateTime(2015, 3, 15), Temat = "COMY", ProwadzacyId = 1, HasloDostepu = "098f6bcd4621d373cade4e832627b4f6", StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety },
            new Warsztat() { Nazwa = "ZSB", CzasTrwania = "1h", DataRozpoczecia = new DateTime(2015, 1, 20), Temat = "Zapory Ogniowe", ProwadzacyId = 1, HasloDostepu = "098f6bcd4621d373cade4e832627b4f6", StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety },
            new Warsztat() { Nazwa = "ZSB", CzasTrwania = "2h", DataRozpoczecia = new DateTime(2015, 2, 20), Temat = "PKI", ProwadzacyId = 1, StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety },
            new Warsztat() { Nazwa = "ZSB", CzasTrwania = "3h", DataRozpoczecia = new DateTime(2015, 3, 20), Temat = "Monitoring", ProwadzacyId = 1, StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety }
            );
            context.SaveChanges();

            context.UdzialyWWarsztacie.AddOrUpdate(d => d.WarsztatId, new UdzialWWarsztacie() { UzytkownikId = 1, WarsztatId = 1, KomentarzProwadzacego = "", Ocena = "zaliczenie" },
            new UdzialWWarsztacie() { UzytkownikId = 1, WarsztatId = 2, KomentarzProwadzacego = "", Ocena = "3.5" },
            new UdzialWWarsztacie() { UzytkownikId = 1, WarsztatId = 4, KomentarzProwadzacego = "", Ocena = "5.0" },
            new UdzialWWarsztacie() { UzytkownikId = 1, WarsztatId = 5, KomentarzProwadzacego = "", Ocena = "5.0" },
            new UdzialWWarsztacie() { UzytkownikId = 1, WarsztatId = 7, KomentarzProwadzacego = "", Ocena = "4.5" },
            new UdzialWWarsztacie() { UzytkownikId = 1, WarsztatId = 8, KomentarzProwadzacego = "", Ocena = "4.0" }
            );
            context.UdzialyWWarsztacie.AddOrUpdate(d => d.WarsztatId, new UdzialWWarsztacie() { UzytkownikId = 2, WarsztatId = 1, KomentarzProwadzacego = "", Ocena = "zaliczenie" },
            new UdzialWWarsztacie() { UzytkownikId = 2, WarsztatId = 2, KomentarzProwadzacego = "", Ocena = "4.5" },
            new UdzialWWarsztacie() { UzytkownikId = 2, WarsztatId = 4, KomentarzProwadzacego = "", Ocena = "3.0" },
            new UdzialWWarsztacie() { UzytkownikId = 3, WarsztatId = 5, KomentarzProwadzacego = "", Ocena = "3.0" },
            new UdzialWWarsztacie() { UzytkownikId = 3, WarsztatId = 7, KomentarzProwadzacego = "", Ocena = "5.5" },
            new UdzialWWarsztacie() { UzytkownikId = 3, WarsztatId = 8, KomentarzProwadzacego = "", Ocena = "3.0" }
            );

            context.SaveChanges();


        }
    }
}
