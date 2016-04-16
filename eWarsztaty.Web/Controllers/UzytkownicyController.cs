using eWarsztaty.Domain;
using eWarsztaty.Web.Infrastructure;
using eWarsztaty.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eWarsztaty.Web.Models.ViewModels;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace eWarsztaty.Web.Controllers
{

    public class UzytkownicyController : Controller
    {
        private eWarsztatyContext _db;

        public UzytkownicyController(eWarsztatyContext db)
        {
            _db = db;
        }

        [RbacAuthorize]
        public ActionResult Index()
        {
            var roleLista = _db.Uzytkownicy;
            var model = (roleLista)
                .Project()
                .To<UzytkownicyListView>().ToList();
            ViewBag.Brak = false;

            foreach (var uzytkownik in model)
            {
                var listaRol = _db.UzytkownicyRole.Include("Uzytkownik").Include("Rola").Where(x => x.UzytkownikId == uzytkownik.UzytkownikId);
                foreach (var item in listaRol)
                {
                    uzytkownik.Role.Add(item.Rola);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Detail(int? uzytkownikId)
        {

            UzytkownikView model = new UzytkownikView();
            var listaRol = _db.Role.ToList();

            if (uzytkownikId != null)
            {
                if (this.HasPermission("Uzytkownicy-Detail")) //edycja roli
                {
                    var uzytkownik = _db.Uzytkownicy.FirstOrDefault(x => x.UzytkownikId == uzytkownikId);

                    model = Mapper.Map<Uzytkownik, UzytkownikView>(uzytkownik);
                    model.Role = listaRol;
                    var roleList = _db.UzytkownicyRole.Where(x => x.UzytkownikId == uzytkownikId).Select(x => x.RolaId).ToArray();

                    model.SelectedRole = roleList;
                    ViewBag.NowyUzytkownik = "Edytuj uzytkownika " + model.Login;
                    ViewBag.Enable = true;
                    ViewBag.Nowy = false;
                }
                else
                    return this.HttpNotFound(); //http not found
            }
            else
            {
                if (this.HasPermission("Uzytkownicy-Save")) //nowa rola z linka
                {
                    model.Role = listaRol;
                    model.SelectedRole = listaRol.Select(x => x.RolaId).ToArray();
                    ViewBag.NowyUzytkownik = "Dodaj nowego uzytkownika";
                    ViewBag.Enable = true;
                    ViewBag.Nowy = true;
                }
                else
                    return this.HttpNotFound(); //http not found
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail()
        {
            if (this.HasPermission("Uzytkownicy-Save")) //nowa rola z linka
            {
                UzytkownikView model = new UzytkownikView();
                var listaRol = _db.Role.ToList();

                model.Role = listaRol;
                model.SelectedRole = listaRol.Select(x => x.RolaId).ToArray();
                ViewBag.NowyUzytkownik = "Dodaj nowego uzytkownika";
                ViewBag.Enable = true;
                ViewBag.Nowy = true;

                return View(model);
            }
            else
                return this.HttpNotFound(); //http not found
        }

        [HttpPost]
        public ActionResult Save(UzytkownikView model)
        {
            var res = new JObject();
            try
            {
            if (model.Role == null)
            {
                model.Role = _db.Role.ToList();
            }

            if (model.UzytkownikId == 0)// nowy uzytkownik 
            {
                if (this.HasPermission("Uzytkownicy-Save"))
                {
                    Uzytkownik uzytkownik = new Uzytkownik();
                    uzytkownik.Login = model.Login;
                    string noweHaslo = CustomMembershipProvider.GetMd5Hash(model.Haslo);
                    uzytkownik.Haslo = noweHaslo;
                    uzytkownik.AdresEmail = model.AdresEmail;
                    _db.AddUser(uzytkownik);
                    _db.SaveChanges();
                    res["ok"] = true;
                    res["Info"] = "użytkownik dodany poprawnie";
                }
                else
                    return this.HttpNotFound(); //http not found
            }
            else
            {
                if (this.HasPermission("Uzytkownicy-Detail")) //edycja roli
                {
                    var user = _db.Uzytkownicy.FirstOrDefault(x => x.UzytkownikId == model.UzytkownikId);
                    user.Login = model.Login;
                    user.AdresEmail = model.AdresEmail;
                    if (model.Haslo != null)
                    {
                        string noweHaslo = CustomMembershipProvider.GetMd5Hash(model.Haslo);
                        user.Haslo = noweHaslo;
                    }
                    res["ok"] = true;
                    res["Info"] = "użytkownik zedytowany poprawnie";
                    _db.SaveChanges();
                }
                else
                    return this.HttpNotFound(); //http not found
            }
            //dodanie roli do uzytkownika
            List<int> role;
            if (model.SelectedRole != null)
            {
                role = new List<int>(model.SelectedRole);
                CustomRoleProvider.AddRolesToUzytkownik(role, model.Login);
            }
            }
            catch (Exception)
            {
                res["ok"] = false;
                res["Info"] = "użytkownik o danym loginie już istnieje";
                
            }
            return Content(res.ToString(), "application/json");
        }

        [RbacAuthorize]
        [HttpGet]
        public ActionResult Delete(int? uzytkownikId)
        {
            if (uzytkownikId != null)
            {
                var uzytkownik = _db.Uzytkownicy.FirstOrDefault(x => x.UzytkownikId == uzytkownikId);
                CustomMembershipProvider.DeleteUzytkownik(uzytkownik.Login);
                _db.SaveChanges();
            }
            return this.RedirectToAction("Index");
        }
    }
}
