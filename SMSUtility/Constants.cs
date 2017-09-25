using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSUtility
{
    public static class Constants
    {
        public const string UserCookie = "UserCookie";
        public const string ChangePwdLoginCookie = "ChangePwdFirstLogin";

        
    }

    #region Enums


    public enum UserType
    {
        Admin = 1,
        General = 2
    }

    #endregion
}
