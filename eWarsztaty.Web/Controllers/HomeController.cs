using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eWarsztaty.Domain;
using eWarsztaty.Web.Infrastructure;
using eWarsztaty.Web.Models;
using System.Data.Entity.SqlServer;
using System.Web.Caching;
using AutoMapper;
using Newtonsoft.Json.Linq;
using eWarsztaty.Web.Models.ViewModels;

namespace eWarsztaty.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private eWarsztatyContext _db;

        public HomeController(eWarsztatyContext db)
        {
            _db = db;
        }


        public ActionResult Home()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult Index()
        {
            var wszystkieWarsztaty = _db.Warsztaty.OrderBy(x => x.Nazwa); //musi byc orderby do grid MVC

            return View(wszystkieWarsztaty);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CurrentRole()
        {
            string userName = User.Identity.Name;
            int userId = _db.Uzytkownicy.FirstOrDefault(x=>x.Login == userName).UzytkownikId;
            List<int> listaId = _db.UzytkownicyRole.Where(x => x.UzytkownikId == userId).Select(x => x.RolaId).ToList();
            var model = new CurrentRoleView()
            {
                UserRoles = GetRoles(listaId)
            };
            var check = HttpContext.Cache["CurrentRoleId"];
            if (check == null || listaId.Contains((int)HttpContext.Cache["CurrentRoleId"]) == false)
            {
                SaveCurrentRole(listaId[0].ToString());
            }
            model.SelectedUserRoleId =(int) HttpContext.Cache["CurrentRoleId"];
            return PartialView("_RolePartial",model);
        }

        private IEnumerable<SelectListItem> GetRoles(List<int> listaId)
        {
            var roles = _db.Role.Where(x=>listaId.Contains(x.RolaId))
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = SqlFunctions.StringConvert((double)x.RolaId).Trim(),
                                    Text = x.Nazwa
                                }).ToList();

            return new SelectList(roles, "Value", "Text");
        }

        public JsonResult SaveCurrentRole(string roleId)
        {
            int rolaId = Int32.Parse(roleId);
            var rola = _db.Role.FirstOrDefault(x => x.RolaId == rolaId);
            HttpContext.Cache["CurrentRoleId"] = rola.RolaId;
            HttpContext.Cache["CurrentRoleName"] = rola.Nazwa;
            return Json(new { redirectUrl = Url.Action("Index", "Home"), isRedirect = true });
        }

        [RbacAuthorize]
        public ActionResult EditProfile(string userName)
        {
            ViewBag.Nowy = false;
            UzytkownikView model = new UzytkownikView();
            if (userName == User.Identity.Name) //edytujemy swoj profil
            {

                var uzytkownik = _db.Uzytkownicy.FirstOrDefault(x => x.Login == userName);
                model = Mapper.Map<Uzytkownik, UzytkownikView>(uzytkownik);

                var listaRol = _db.Role.ToList();
                model.Role = listaRol;
                var roleList = _db.UzytkownicyRole.Where(x => x.UzytkownikId == uzytkownik.UzytkownikId).Select(x => x.RolaId).ToArray();
                model.SelectedRole = roleList;
            }
            else
                return RedirectToAction("Index", "Forbidden");
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveProfile(UzytkownikView model)
        {
            var res = new JObject();
            if (model.Role == null || model.Role.Count == 0)
            {
                model.Role = _db.Role.ToList();
            }

            //if (model.Login != User.Identity.Name)
            //{
            //    res["ok"] = true;
            //    res["Info"] = "zapis udany";
            //}
            //else
            //{
            //    if (model.Haslo ==null)
            //    {
            //        res["ok"] = false;
            //        res["Info"] = "pole Hasło nie może byc puste";
            //        return Content(res.ToString(), "application/json");
            //    }
                if (this.HasPermission("Home-EditProfile")) //edycja roli
                {
                    var user = _db.Uzytkownicy.FirstOrDefault(x => x.UzytkownikId == model.UzytkownikId);
                    user.Login = model.Login;
                    user.AdresEmail = model.AdresEmail;
                    if (model.Haslo != null)
                    {
                        string noweHaslo = CustomMembershipProvider.GetMd5Hash(model.Haslo);
                        user.Haslo = noweHaslo;
                    }
                    _db.SaveChanges();
                }
                else{
                    return this.HttpNotFound() ;
                }
            //}
            //dodanie roli do uzytkownika
            List<int> role;
            if (model.SelectedRole != null){
                role = new List<int>(model.SelectedRole);
                CustomRoleProvider.AddRolesToUzytkownik(role, model.Login);
            }
            res["ok"] = true;
            res["Info"] = "edycja profilu zakończona";
            return Content(res.ToString(), "application/json");
        }

    }
}
