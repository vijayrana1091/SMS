using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSEntities;
using SMSUtility;
using System.Web.Routing;

namespace SMS.Filters
{
    public class AuthorizeFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        private bool IsUserValid()
        {
            bool isValid = false;
            isValid = HttpContext.Current.Request.IsAuthenticated;
            return isValid;
        }

        private bool IsUserAdmin()
        {
            bool isAdmin = false;
            string encryptedLoginRes = HttpContext.Current.Request.Cookies[Constants.UserCookie].Value;
            string decryptedLoginRes = Common.Decrypt(encryptedLoginRes);
            if (!string.IsNullOrEmpty(decryptedLoginRes))
            {
                string[] cookieValue = decryptedLoginRes.Split('|');
                LoginResponse userData = new LoginResponse()
                {
                    I_USER_TYPE = Convert.ToInt16(cookieValue[4])
                };
                if (userData.I_USER_TYPE == (int)UserType.Admin)
                    isAdmin = true;
            }
            return isAdmin;
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
                else if (IsUserValid() && HttpContext.Current.Request.Cookies[Constants.UserCookie] != null && !IsUserAdmin())
                {
                    HandleUnauthorizedRequest(filterContext);
                }
            }
        }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                                 new RouteValueDictionary
                                   {
                                       { "action", "Error" },
                                       { "controller", "Home" }
                                   });
        }
    }
}