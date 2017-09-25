using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSEntities
{
   
    public class LoginResponse
    {
      
        public int I_USER_ID { get; set; }

       
        public string V_FIRST_NAME { get; set; }

       
        public string V_LAST_NAME { get; set; }

      
        public string V_TITLE { get; set; }

       
        public string V_EMAIL { get; set; }

        
        public string V_ADDRESS1 { get; set; }

       
        public string V_ADDRESS2 { get; set; }

      
        public string V_CITY { get; set; }

        
        public string I_ZIP { get; set; }

       
        public Int16 I_COUNTRY_ID { get; set; }

       
        public string V_COMPANY { get; set; }

       
        public string V_PHONE_NO { get; set; }

        
        public string V_HOW_CAN { get; set; }

       
        public Int16 I_USER_TYPE { get; set; }

       
        public bool B_IS_ENABLE { get; set; }

      
        public DateTime D_REQUESTED_DATE { get; set; }

      
        public bool B_IS_REGISTERED { get; set; }

      
        public DateTime? D_LAST_LOGIN_DATE { get; set; }

        
        public string V_USER_PASSWORD { get; set; }

       
        public bool B_ISVALIDLOGIN { get; set; }

       
        public string V_INVALIDLOGINREASON { get; set; }

       
        public DateTime D_CHANGE_PASSWORD_DATE { get; set; }


    }
}
