using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSEntities;
using System.Configuration;
using System.Text;
using SMSUtility;
using System.Web.Security;

namespace SMS.Controllers
{
    public class BaseController : Controller
    {
        public void AddCookies(LoginResponse loginRes)
        {
            if (loginRes != null)
            {
                int timeOutPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOutInMinutes"]);
                string encryptedLoginRes = string.Empty;
                StringBuilder sbLoginRes = new StringBuilder(200);
                sbLoginRes.Append(loginRes.B_IS_ENABLE).Append("|")
                    .Append(loginRes.B_IS_REGISTERED).Append("|")
                    .Append(loginRes.D_LAST_LOGIN_DATE).Append("|")
                    .Append(loginRes.I_USER_ID).Append("|")
                    .Append(loginRes.I_USER_TYPE).Append("|")
                    .Append(loginRes.V_EMAIL).Append("|")
                    .Append(loginRes.V_FIRST_NAME).Append("|")
                    .Append(loginRes.V_LAST_NAME).Append("|")
                    .Append(loginRes.V_TITLE).Append("|")
                    .Append(loginRes.B_ISVALIDLOGIN).Append("|")
                    .Append(loginRes.V_COMPANY);

                encryptedLoginRes = Common.Encrypt(sbLoginRes.ToString());

                HttpCookie userCookie = new HttpCookie(Constants.UserCookie, encryptedLoginRes)
                {
                    Expires = System.DateTime.Now.AddMinutes(timeOutPeriod),
                    HttpOnly = true
                };
                Response.Cookies.Add(userCookie);
            }
        }

        private void UpdateCookies()
        {
            if (Request.Cookies[Constants.UserCookie] != null)
            {
                int timeOutPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOutInMinutes"]);
                HttpCookie userCookie = new HttpCookie(Constants.UserCookie, Request.Cookies[Constants.UserCookie].Value)
                {
                    Expires = System.DateTime.Now.AddMinutes(timeOutPeriod),
                    HttpOnly = true
                };
                Response.Cookies.Add(userCookie);
            }
        }

        public void DeleteCookies(string cookieType)
        {
            if (cookieType == Constants.UserCookie)
            {
                if (Request.Cookies[Constants.UserCookie] != null)
                {
                    HttpCookie userCookie = new HttpCookie(Constants.UserCookie)
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    Response.Cookies.Add(userCookie);
                }

                FormsAuthentication.SignOut();

                HttpCookie formCokie = Request.Cookies[FormsAuthentication.FormsCookieName];
                formCokie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(formCokie);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetAllowResponseInBrowserHistory(false);
            }
            else if (cookieType == Constants.ChangePwdLoginCookie)
            {
                if (Request.Cookies[Constants.ChangePwdLoginCookie] != null)
                {
                    HttpCookie userCookieChange = new HttpCookie(Constants.ChangePwdLoginCookie)
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    Response.Cookies.Add(userCookieChange);
                }
            }
        }

        public LoginResponse ReadCookie()
        {
            LoginResponse userData = new LoginResponse();
            if (Request.Cookies[Constants.UserCookie] != null)
            {
                string encryptedLoginRes = Request.Cookies[Constants.UserCookie].Value;

                string decryptedLoginRes = Common.Decrypt(encryptedLoginRes);

                if (!string.IsNullOrEmpty(decryptedLoginRes))
                {
                    string[] cookieValue = decryptedLoginRes.Split('|');

                    userData.B_IS_ENABLE = !string.IsNullOrEmpty(cookieValue[0]) ? Convert.ToBoolean(cookieValue[0]) : false;
                    userData.B_IS_REGISTERED = !string.IsNullOrEmpty(cookieValue[1]) ? Convert.ToBoolean(cookieValue[1]) : false;
                    userData.D_LAST_LOGIN_DATE = Convert.ToDateTime(cookieValue[2]);
                    userData.I_USER_ID = Convert.ToInt32(cookieValue[3]);
                    userData.I_USER_TYPE = Convert.ToInt16(cookieValue[4]);
                    userData.V_EMAIL = Convert.ToString(cookieValue[5]);
                    userData.V_FIRST_NAME = Convert.ToString(cookieValue[6]);
                    userData.V_LAST_NAME = Convert.ToString(cookieValue[7]);
                    userData.V_TITLE = Convert.ToString(cookieValue[8]);
                    userData.B_ISVALIDLOGIN = !string.IsNullOrEmpty(cookieValue[9]) ? Convert.ToBoolean(cookieValue[9]) : false;
                    userData.V_COMPANY = cookieValue[10];
                }
            }
            return userData;
        }

        public LoginResponse CurrentUser
        {
            get
            {
                LoginResponse currentUser = ReadCookie();
                if (currentUser != null && currentUser.V_FIRST_NAME != null)
                {
                    UpdateCookies();
                }
                return currentUser;
            }
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public void AddCookiesForChangePassword(LoginResponse loginRes)
        {
            if (loginRes != null)
            {
                int timeOutPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOutInMinutes"]);
                string encryptedLoginRes = string.Empty;

                StringBuilder sbLoginRes = new StringBuilder
                {
                    Capacity = 130
                };
                sbLoginRes.Append(loginRes.V_EMAIL).Append("|")
                    .Append(loginRes.V_INVALIDLOGINREASON);

                encryptedLoginRes = Common.Encrypt(sbLoginRes.ToString()); //Convert.ToString(encryptedBytes);// encryptedLoginRes.TrimEnd('~');

                HttpCookie userCookieChange = new HttpCookie(Constants.ChangePwdLoginCookie, encryptedLoginRes)
                {
                    Expires = System.DateTime.Now.AddMinutes(timeOutPeriod),
                    HttpOnly = true
                };
                Response.Cookies.Add(userCookieChange);
            }
        }

        public string[] ReadCookieForChangePassword()
        {
            if (Request.Cookies[Constants.ChangePwdLoginCookie] != null)
            {
                string encryptedLoginRes = Request.Cookies[Constants.ChangePwdLoginCookie].Value;

                string decryptedLoginRes = Common.Decrypt(encryptedLoginRes);

                if (!string.IsNullOrEmpty(decryptedLoginRes))
                {
                    StringBuilder sbLoginRes = new StringBuilder
                    {
                        Capacity = 130
                    };
                    string[] userData = decryptedLoginRes.Split('|');
                    return userData;
                }
            }
            return null;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            else
            {
                string actionName = filterContext.RouteData.Values["action"].ToString();
                Type controllerType = filterContext.Controller.GetType();
                var method = controllerType.GetMethod(actionName);
                var returnType = method.ReturnType;

                //If the action that generated the exception returns JSON
                if (returnType.Equals(typeof(JsonResult)))
                {
                    filterContext.Result = new JsonResult()
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            error = true,
                            message = filterContext.Exception.Message
                        }
                    };

                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = 900;
                }

                //If the action that generated the exception returns a view
                if (returnType.Equals(typeof(ActionResult)) || (returnType).IsSubclassOf(typeof(ActionResult)))
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = @"Home\Error"
                    };
                }
              
            }
        }
    }
}