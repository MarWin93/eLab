using eWarsztaty.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eWarsztaty.Web.Models.ViewModels;

namespace eWarsztaty.Web.Controllers
{
    [eWarsztatyAuthorize(Roles = "Admin")]
    public class UprawnieniaController : Controller
    {
        //
        // GET: /Uprawnienia/

        public ActionResult Index()
        {
            return View();
        }

    }
}
