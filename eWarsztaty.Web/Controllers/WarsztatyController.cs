using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Helpers;
using eWarsztaty.Web.Infrastructure;
using eWarsztaty.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eWarsztaty.Web.Models.ViewModels;
using AutoMapper.QueryableExtensions;
using System.IO;

namespace eWarsztaty.Web.Controllers
{
    [eWarsztatyAuthorize]
    public class WarsztatyController : Controller
    {
        private eWarsztatyContext _db;

        public WarsztatyController(eWarsztatyContext db)
        {
            _db = db;
        }

        [RbacAuthorize]
        public ActionResult Index()
        {

            var warsztatyLista = _db.Warsztaty.ToList();
            IList<WarsztatyListView> model = Mapper.Map<IList<Warsztat>, IList<WarsztatyListView>>(warsztatyLista);

            foreach (var warsztat in model)
            {
                //var listaRol = _db.UzytkownicyRole.Where(x => x.UzytkownikId == uzytkownik.UzytkonikId).ToList();
                //foreach (var item in listaRol)
                //{
                //    uzytkownik.Role.Add(_db.Role.FirstOrDefault(x => x.RolaId == item.RolaId));
                //}
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Detail(int? warsztatId)
        {

            WarsztatyListView model = new WarsztatyListView();

            if (warsztatId != null)
            {
                if (this.HasPermission("Warsztaty-Detail")) //edycja warsztatu
                {
                    var warsztat = _db.Warsztaty.FirstOrDefault(x => x.WarsztatId == warsztatId);

                    model = Mapper.Map<Warsztat, WarsztatyListView>(warsztat);

                    ViewBag.NowyWarsztat = "Edytuj Warsztat" + model.Nazwa;
                    ViewBag.Enable = true;
                    ViewBag.Nowy = false;
                }
                else
                    return this.HttpNotFound(); //http not found
            }
            else
            {
                if (this.HasPermission("Warsztaty-Save")) //nowa warsztat z linka
                {
                    ViewBag.NowyUzytkownik = "Dodaj nowy warsztat";
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
            if (this.HasPermission("Warsztaty-Save")) //nowy warsztat  z linka
            {

                WarsztatyListView model = new WarsztatyListView();



                ViewBag.NowyUzytkownik = "Dodaj nowego uzytkownika";
                ViewBag.Enable = true;
                ViewBag.Nowy = true;

                return View(model);
            }
            else
                return this.HttpNotFound(); //http not found
        }

        [HttpPost]
        public ActionResult Save(WarsztatyListView model)
        {
            var res = new JObject();
            try
            {
                if (model.WarsztatId == 0)// nowy Warsztat
                {
                    if (this.HasPermission("Warsztaty-Save"))
                    {
                        Warsztat warsztat = new Warsztat();
                        warsztat = Mapper.Map<WarsztatyListView, Warsztat>(model);
                        warsztat.ProwadzacyId = this.CurrentUserId();
                        warsztat.HasloDostepu = CustomMembershipProvider.GetMd5Hash(model.HasloDostepu);
                        _db.Warsztaty.Add(warsztat);
                        //musimy stworzyc automatycnzie udzial w tym warsztacie
                        var udzial = new UdzialWWarsztacie() { Aktywny = false, UzytkownikId = warsztat.ProwadzacyId, Warsztat = warsztat };
                        _db.UdzialyWWarsztacie.Add(udzial);
                        _db.SaveChanges();
                        res["ok"] = true;
                        res["Info"] = "dodano nowy warsztat";
                    }
                    else
                        return this.HttpNotFound(); //http not found
                }
                else
                {
                    if (this.HasPermission("Warsztaty-Detail")) //edycja warsztatu
                    {
                        var warsztat = _db.Warsztaty.FirstOrDefault(x => x.WarsztatId == model.WarsztatId);
                        warsztat.Nazwa = model.Nazwa;
                        warsztat.Temat = model.Temat;
                        warsztat.HasloDostepu = CustomMembershipProvider.GetMd5Hash(model.HasloDostepu);
                        warsztat.DataRozpoczecia = model.DataRozpoczecia;
                        warsztat.CzasTrwania = model.CzasTrwania;
                        _db.SaveChanges();
                        res["ok"] = true;
                        res["Info"] = "warsztat zedytowany poprawnie";
                    }
                    else
                        return this.HttpNotFound(); //http not found
                }
            }
            catch (Exception ex)
            {
                res["ok"] = false;
                res["Info"] = "Warsztat o danej nazwie już istnieje";
            }
            return Content(res.ToString(), "application/json");
        }

        [RbacAuthorize]
        [HttpGet]
        public ActionResult Delete(int? warsztatId, string url)
        {
            if (warsztatId != null)
            {
                var warsztat = _db.Warsztaty.FirstOrDefault(x => x.WarsztatId == warsztatId);
                _db.Warsztaty.Remove(warsztat);
                var warsztatUdzialy = _db.UdzialyWWarsztacie.Where(x => x.WarsztatId == warsztatId).ToList();
                foreach (var udzial in warsztatUdzialy)
                {
                    _db.UdzialyWWarsztacie.Remove(udzial);
                }

                _db.SaveChanges();
            }
            return this.RedirectToAction(url);
        }

        [RbacAuthorize]
        public ActionResult MojeWarsztaty()
        {
            int userId = _db.Uzytkownicy.FirstOrDefault(x => x.Login == User.Identity.Name).UzytkownikId;
            var udzialyWwarsztatach = _db.UdzialyWWarsztacie.Where(x => x.UzytkownikId == userId).ToList();
            if (udzialyWwarsztatach != null)
            {
                List<int> listaId = _db.UdzialyWWarsztacie.Where(x => x.UzytkownikId == userId).Select(x => x.WarsztatId).ToList();

                var warsztatyLista = _db.Warsztaty.Where(x => listaId.Contains(x.WarsztatId));
                var model = (warsztatyLista)
                    .Project()
                    .To<MojeWarsztatyListView>();
                ViewBag.Brak = false;
                return View(model.ToList());
            }
            else
            {
                var model = (_db.Warsztaty)
                    .Project()
                    .To<MojeWarsztatyListView>();
                ViewBag.Brak = true;
                return View(model.ToList());
            }

        }

        public ActionResult Oceny(int? WarsztatId)
        {
            OcenyView model = new OcenyView();
            if (WarsztatId != null)
            {
                var udzialyWwarsztatach = _db.UdzialyWWarsztacie.Where(x => x.WarsztatId == WarsztatId).ToList();
                List<int> listaId = _db.UdzialyWWarsztacie.Where(x => x.WarsztatId == WarsztatId).Select(x => x.UzytkownikId).ToList();
                model.WarsztatId = (int)WarsztatId;
                model.Uzytkownicy = _db.Uzytkownicy.Where(x => listaId.Contains(x.UzytkownikId)).ToList();
                model.Oceny = new string[listaId.Count];
                for (int i = 0; i < listaId.Count; i++)
                {
                    model.Oceny[i] = udzialyWwarsztatach[i].Ocena;
                }
            }

            return View(model);
        }

        public ActionResult OcenySave(OcenyView model)
        {
            var udzialyWwarsztatach = _db.UdzialyWWarsztacie.Where(x => x.WarsztatId == model.WarsztatId).ToList();
            for (int i = 0; i < model.Oceny.Length; i++)
            {
                udzialyWwarsztatach[i].Ocena = model.Oceny[i];
            }
            _db.SaveChanges();
            return RedirectToAction("MojeWarsztaty", "Warsztaty");
        }

        public ActionResult Zapisanie()
        {
            if (this.HasPermission("Warsztaty-Zapis")) //edycja warsztatu
            {
                List<int> listaId = _db.Warsztaty.Select(x => x.WarsztatId).ToList();
                ZapisanieView model = new ZapisanieView() { WszystkieWarsztaty = GetWarsztaty(listaId) };

                return View(model);
            }
            return this.HttpNotFound(); //http not found
        }

        [RbacAuthorize]
        public ActionResult Zapis(ZapisanieView model)// ZABEZPIECZENIE ZEBY 2 RAZY NA TEN SAM WARSZTAT SIE NIE ZAPISAC
        {
            var res = new JObject();
            int curruserid = this.CurrentUserId();
            if (_db.UdzialyWWarsztacie.FirstOrDefault(x => x.WarsztatId == model.SelectedWarsztatId && x.UzytkownikId == curruserid) == null)
            {
                if (CustomMembershipProvider.GetMd5Hash(model.HasloDostepu) == _db.Warsztaty.FirstOrDefault(x => x.WarsztatId == model.SelectedWarsztatId).HasloDostepu)
                {
                    UdzialWWarsztacie udzial = new UdzialWWarsztacie() { WarsztatId = model.SelectedWarsztatId, UzytkownikId = this.CurrentUserId() };
                    _db.UdzialyWWarsztacie.Add(udzial);
                    _db.SaveChanges();
                    res["ok"] = true;
                    res["Info"] = "zapis udany";
                }//redirect ze bledne haslo
                else
                {
                    res["ok"] = false;
                    res["Info"] = "hasło do warsztatu jest nieprawidłowe";
                }
            }
            else
            {
                res["ok"] = false;
                res["Info"] = "Jesteś już zapisany na ten wykład";
            }
            //redirect ze jestes juz zapisany na ten wyklad
            return Content(res.ToString(), "application/json");
        }

        [RbacAuthorize]
        public ActionResult Wypisanie(int? WarsztatId)
        {
            if (WarsztatId != null)
            {
                int userId = DBHelpers.GetUserId(User.Identity.Name);
                var udzial = _db.UdzialyWWarsztacie.FirstOrDefault(x => x.WarsztatId == WarsztatId && x.UzytkownikId == userId);
                _db.UdzialyWWarsztacie.Remove(udzial);
                _db.SaveChanges();
            }

            return RedirectToAction("MojeWarsztaty", "Warsztaty");
        }



        #region Prowadzenie warsztatu

        [HttpPost]
        public JsonResult CanJoinToWorshop()
        {
            ////sprawdzenie czy uzytkownik bierze aktywny udzial w innym warsztacie
            //int userId = Helpers.DBHelpers.GetUserId(User.Identity.Name);
            //var udzial = _db.UdzialyWWarsztacie.Include("Warsztat").Where(x => x.UzytkownikId == userId && x.Warsztat.StatusWarsztatu == (int) eWarsztatyEnums.StatusWarsztatu.WTrakcie);
            //if (udzial != null)
            //    return Json(new { isOk = false, message = "Nie możesz dołączyć do warsztatu. Bierzesz udział w innym o nazwie: " + udzial.Warsztat.Nazwa });
            //else
                return Json(new { isOk = true});
            
        }
        public ActionResult Warsztat(int warsztatId)
        {
            var warsztatDb = _db.Warsztaty.FirstOrDefault(x => x.WarsztatId == warsztatId);
            if (warsztatDb != null)
            {
                WarsztatyView warsztatModel = new WarsztatyView() { Nazwa = warsztatDb.Nazwa, ProwadzacyId = warsztatDb.ProwadzacyId, Temat = warsztatDb.Temat, WarsztatId = warsztatDb.WarsztatId };
                if (DBHelpers.GetUserId(User.Identity.Name) == warsztatDb.ProwadzacyId && warsztatDb.StatusWarsztatu == (int)eWarsztatyEnums.StatusWarsztatu.Zamkniety) // prowadzacy
                {
                    warsztatDb.StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.WTrakcie;
                    _db.SaveChanges();
                    return View(warsztatModel);
                }
                else if (warsztatDb.StatusWarsztatu == (int)eWarsztatyEnums.StatusWarsztatu.WTrakcie)
                {
                    return View(warsztatModel);
                }
            }
            return this.RedirectToAction("MojeWarsztaty");
        }

        public ActionResult GetParticipants(int warsztatId)
        {
            var listaDb = _db.UdzialyWWarsztacie.Include("Uzytkownik").Where(x => x.WarsztatId == warsztatId && x.Aktywny == true).ToList();

            List<UczestnicyListView> listaModel = new List<UczestnicyListView>();
            foreach (var item in listaDb)
            {
                if (item.Warsztat.ProwadzacyId != item.UzytkownikId)
                {
                    UczestnicyListView poz = new UczestnicyListView() { Login = item.Uzytkownik.Login, UczestnikId = item.UzytkownikId, AgentAktywny = item.AktywnyAgent };
                    listaModel.Add(poz);
                }
            }
            return PartialView("_Uczestnicy", listaModel);
        }

        #endregion

        #region Upload

        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase file, int warsztatId, bool isFile = true)
        {
            if (file == null && file.ContentLength < 0)
                return Json(new { isOk = false });

            //przygotowanie do zapisu w bazie
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] data = target.ToArray();
            string extension = Path.GetExtension(file.FileName);
            Plik plik = new Plik() { File = data, Nazwa = file.FileName, WarsztatId = warsztatId, Rozszerzenie = extension, Zadanie = !isFile };
            _db.Pliki.Add(plik);
            _db.SaveChanges();

            return Json(new { isOk = true });
        }

        public ActionResult GetFiles(int warsztatId)
        {
            var listaDb = _db.Pliki.Include("Warsztat").Where(x => x.WarsztatId == warsztatId && x.Zadanie == false);

            var model = (listaDb)
                .Project()
                .To<PlikiListView>();

            return PartialView("_Pliki", model.ToList());
        }

        public ActionResult GetExercises(int warsztatId)
        {
            var listaDb = _db.Pliki.Include("Warsztat").Where(x => x.WarsztatId == warsztatId && x.Zadanie == true);

            var model = (listaDb)
                .Project()
                .To<PlikiListView>();

            return PartialView("_Zadania", model.ToList());
        }

        public ActionResult SaveFile(int plikId)
        {
            Plik plik = _db.Pliki.FirstOrDefault(x => x.PlikId == plikId);

            if (plik == null)
            {
                return HttpNotFound();
            }

            return File(plik.File, "content/type", plik.Nazwa);

        }

        public ActionResult Prezentacja(int plikId)
        {

            Plik plik = _db.Pliki.FirstOrDefault(x => x.PlikId == plikId);

            if (plik == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Prezentacja", Convert.ToBase64String(plik.File));
        }

        #endregion


        private IEnumerable<System.Web.Mvc.SelectListItem> GetWarsztaty(List<int> listaId)
        {
            var warsztaty = _db.Warsztaty.Where(x => listaId.Contains(x.WarsztatId))
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = SqlFunctions.StringConvert((double)x.WarsztatId).Trim(),
                                    Text = x.Nazwa + "- " + x.Temat
                                }).ToList();

            return new SelectList(warsztaty, "Value", "Text");
        }
    }
}
