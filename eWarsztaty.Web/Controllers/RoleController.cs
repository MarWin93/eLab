using AutoMapper;
using eWarsztaty.Domain;
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

namespace eWarsztaty.Web.Controllers
{
    public class RoleController : Controller
    {
        private eWarsztatyContext _db;

        public RoleController(eWarsztatyContext db)
        {
            _db = db;
        }

        [RbacAuthorize]
        public ActionResult Index()
        {
            var roleLista = _db.Role.ToList();
            IList<RoleListView> model = Mapper.Map<IList<Rola>, IList<RoleListView>>(roleLista);
            return View(model);
        }

        [HttpGet]
        public ActionResult Detail(int? roleId)
        {
            RoleView model = new RoleView();
            var listaUprawnien = _db.Uprawnienia.ToList();

            if (roleId != null)
            {
                if (this.HasPermission("Role-Detail")) //edycja roli
                {
                    var rola = _db.Role.FirstOrDefault(x => x.RolaId == roleId);
                    if (rola != null)
                    {
                        model = Mapper.Map<Rola, RoleView>(rola);
                        model.Uprawnienia = listaUprawnien;
                        var privilingesList = _db.UprawnieniaRole.Where(x => x.RolaId == roleId).Select(x => x.UprawnienieId).ToArray();

                        model.SelectedUprawnienia = privilingesList;
                        ViewBag.NowaRola = "Edytuj role " + model.Nazwa;
                        ViewBag.Enable = true;
                    }
                }
                else
                    return this.HttpNotFound();
            }
            else
	        {
                if (this.HasPermission("Role-Save")) //nowa rola z linka
                {
                    model.Uprawnienia = listaUprawnien;
                    model.SelectedUprawnienia = new int[] { };
                    ViewBag.NowaRola = "Dodaj nową role";
                    ViewBag.Enable = true;
                }
                else
                    return this.HttpNotFound();
	        }
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail()
        {
            if (this.HasPermission("Role-Save")) //nowa rola z guzika
            {

                RoleView model = new RoleView();
                var listaUprawnien = _db.Uprawnienia.ToList();
                model.Uprawnienia = listaUprawnien;
                model.SelectedUprawnienia = new int[] { };
                ViewBag.NowaRola = "Dodaj nową role";
                ViewBag.Enable = true;

                return View(model);
            }
            else
                return this.HttpNotFound(); //http not found
        }

        [HttpPost]
        public ActionResult Save(RoleView model)
        {
            var res = new JObject();
            try
            { 
                if (model.RolaId == 0)// nowa rola 
	            {
                    if (this.HasPermission("Role-Save"))
                    {
                        Rola rola = new Rola();
                        rola.Nazwa = model.Nazwa;
                        rola.Opis = model.Opis;
                        _db.Role.Add(rola);
                        _db.SaveChanges();
                        res["ok"] = true;
                        res["Info"] = "zapis udany";
                    }
                    else
                        return this.HttpNotFound();
                }
                else
                {
                    if (this.HasPermission("Role-Detail"))
                    {
                        var rola = _db.Role.FirstOrDefault(x => x.RolaId == model.RolaId);
                        rola.Nazwa = model.Nazwa;
                        rola.Opis = model.Opis;
                        _db.SaveChanges();
                        res["ok"] = true;
                        res["Info"] = "Rola zedytowana poprawnie";
                    }
                    else
                        return this.HttpNotFound();
                }
                //dodanie uprawnien do roli
                List<int> uprawnienia;
                if(model.SelectedUprawnienia != null)
                    uprawnienia = new List<int>(model.SelectedUprawnienia);
                else
                    uprawnienia = new List<int>();
                CustomRoleProvider.AddPrivilegesToRole(uprawnienia, model.Nazwa);
                //return this.RedirectToAction("Index");
            }
            catch(Exception ex){
                res["ok"] = false;
                res["Info"] = "Rola o tej nazwie już istnieje";
            }
            return Content(res.ToString(), "application/json");
        }

        [RbacAuthorize]
        [HttpGet]
        public ActionResult Delete (int? roleId)
        {
            if (roleId != null)
            {
                var rola = _db.Role.FirstOrDefault(x => x.RolaId == roleId);
                CustomRoleProvider.DeleteRole(rola.Nazwa);
                _db.SaveChanges();
            }
            return this.RedirectToAction("Index");
        }

        public ActionResult AddRoleToUser()
        {
            AddRoleToUserView model = new AddRoleToUserView();
            if (this.HasPermission("Uzytkownicy-PrzypiszRole"))
	        {
                List<int> listaIdRole = _db.Role.Select(x => x.RolaId).ToList();
                List<int> listaIdUzytkownicy = _db.Uzytkownicy.Select(x => x.UzytkownikId).ToList();

                model = new AddRoleToUserView() { Role = GetRole(listaIdRole), Uzytkownicy = GetUzytkownicy(listaIdUzytkownicy) };
	        }
            else
                return this.HttpNotFound(); //http not found
            
            return View(model);
        }

        public ActionResult SaveRoleToUser(AddRoleToUserView model)
        {
            var res = new JObject();
            if (this.HasPermission("Uzytkownicy-PrzypiszRole"))
            {

                if (model.UserName != null && model.RoleName != null)
                {
                    var user = _db.Uzytkownicy.FirstOrDefault(x => x.Login == model.UserName);
                    var role = _db.Role.FirstOrDefault(x => x.Nazwa == model.RoleName);
                    if (user == null || role == null)
                    {
                        res["ok"] = false;
                        res["Info"] = "błąd";
                    }
                    else if (user != null && role != null)
                    {
                        CustomRoleProvider.AddUserToRole(user.Login, role.Nazwa);
                        res["ok"] = true;
                        res["Info"] = "dodano rolę użytkownikowi";
                    }
                }
                else if (model.UserName != null)
                {
                    var user = _db.Uzytkownicy.FirstOrDefault(x => x.Login == model.UserName);
                    var role = _db.Role.FirstOrDefault(x => x.RolaId == model.SelectedRoleId);
                    if (user == null)
                    {
                        res["ok"] = false;
                        res["Info"] = "błąd";
                    }
                    else
                    {
                        CustomRoleProvider.AddUserToRole(user.Login, role.Nazwa);
                        res["ok"] = true;
                        res["Info"] = "dodano rolę użytkownikowi";
                    }
                }
                else if (model.RoleName != null)
                {
                    var user = _db.Uzytkownicy.FirstOrDefault(x => x.UzytkownikId == model.SelectedUserId);
                    var role = _db.Role.FirstOrDefault(x => x.Nazwa == model.RoleName);
                    if (role == null)
                    {
                        res["ok"] = false;
                        res["Info"] = "błąd";
                    }
                    else
                    {
                        CustomRoleProvider.AddUserToRole(user.Login, role.Nazwa);
                        res["ok"] = true;
                        res["Info"] = "dodano rolę użytkownikowi";
                    }
                }
                else if (model.SelectedUserId != 0 && model.SelectedRoleId != 0)
                {
                    var user = _db.Uzytkownicy.FirstOrDefault(x => x.UzytkownikId == model.SelectedUserId);
                    var role = _db.Role.FirstOrDefault(x => x.RolaId == model.SelectedRoleId);
                    CustomRoleProvider.AddUserToRole(user.Login, role.Nazwa);
                    res["ok"] = true;
                    res["Info"] = "dodano rolę użytkownikowi";
                }
                else
                {
                    return this.HttpNotFound(); 
                }
                return Content(res.ToString(), "application/json");
            }

            else
                return this.HttpNotFound(); //http not found

            return View(model);
        }

        private IEnumerable<System.Web.Mvc.SelectListItem> GetRole(List<int> listaId)
        {
            var role = _db.Role.Where(x => listaId.Contains(x.RolaId))
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = SqlFunctions.StringConvert((double)x.RolaId).Trim(),
                                    Text = x.Nazwa
                                }).ToList();

            return new SelectList(role, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetUzytkownicy(List<int> listaId)
        {
            var uzytkownicy = _db.Uzytkownicy.Where(x => listaId.Contains(x.UzytkownikId))
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = SqlFunctions.StringConvert((double)x.UzytkownikId).Trim(),
                                    Text = x.Login
                                }).ToList();

            return new SelectList(uzytkownicy, "Value", "Text");
        }
    }
}
