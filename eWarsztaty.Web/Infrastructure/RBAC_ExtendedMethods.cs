using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.Routing;

namespace eWarsztaty.Web.Infrastructure
{
    public static class RBAC_ExtendedMethods
    {
        public static bool HasPermission(this ControllerBase controller, string permission)
        {
            bool Found = false;
            try
            {
                int currentRoleId = (int)controller.ControllerContext.HttpContext.Cache.Get("CurrentRoleId");
                Found = CustomRoleProvider.isPermissionInRole(permission, currentRoleId);
            }
            catch(Exception ex) {

                }
            return Found;
        }

        public static int CurrentUserId(this ControllerBase controller)
        {
            int currentUserId = 0; ;
            try
            {
                currentUserId = CustomRoleProvider.GetUserId(controller.ControllerContext.HttpContext.User.Identity.Name); 
            }
            catch (Exception ex)
            {

            }
            return currentUserId;
        }
    }
}