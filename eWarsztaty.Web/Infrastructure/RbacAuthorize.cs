using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using eWarsztaty.Domain;
using eWarsztaty.Web.Infrastructure;
using System.Web.Caching;
using System.Web.Routing;


namespace eWarsztaty.Web.Infrastructure
{
    public class RbacAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string requiredPermission = String.Format("{0}-{1}",
               filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
               filterContext.ActionDescriptor.ActionName);

            string userName = filterContext.RequestContext.HttpContext.User.Identity.Name;
            if (userName == "" || userName == null) //poprawki na niezalogowanego
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                int currentRoleId = (int)filterContext.HttpContext.Cache.Get("CurrentRoleId");
                bool hasPersmission = CustomRoleProvider.isPermissionInRole(requiredPermission, currentRoleId);
                if (!hasPersmission)
                {
                    if (filterContext.HttpContext.Request.IsAuthenticated)
                    {
                        filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.NotFound);
                       // filterContext.Result = new RedirectToRouteResult(new
                        //RouteValueDictionary(new { controller = "Forbidden", action = "Index" }));
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
    }
}