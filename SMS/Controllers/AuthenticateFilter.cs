using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSUtility;
using System.Web.Routing;

namespace SMS.Filters
{
    public class AuthenticateFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        private bool IsUserValid()
        {
            bool isValid = false;
            isValid = HttpContext.Current.Request.IsAuthenticated;
            return isValid;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (!skipAuthorization)
            {
                //Start code to check If cookie has been exipred then redirect to login page
                if (IsUserValid() && HttpContext.Current.Request.Cookies[Constants.UserCookie] == null)
                {
                    HandleUnauthorizedRequest(filterContext);
                }
                //End code
                if (!IsUserValid())
                    HandleUnauthorizedRequest(filterContext);
            }
        }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                                 new RouteValueDictionary
                                   {
                                       { "action", "Index" },
                                       { "controller", "Home" }
                                   });
        }
    }
}