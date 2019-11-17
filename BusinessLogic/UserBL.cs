using BusinessObject;
using System;

namespace BusinessLogic
{
    public class UserBL
    {
        public bool LoginUser(LoginObject loginObject)
        {
            try
            {
                if (loginObject.UserName == "admin" && loginObject.Password == "Admin")
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());

            }
            return false;
        }
    }
}
