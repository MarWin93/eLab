using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using eWarsztaty.Domain;
using eWarsztaty.Web.Infrastructure;


namespace eWarsztaty.Web.Infrastructure
{
    public class eWarsztatyAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
                //filterContext.Result = new RedirectToRouteResult(new
                //RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
        }
    }
}