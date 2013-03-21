using System;
using System.Web;

namespace SimplReaderBLL.BLL.Membership
{
    public class AuthenticationProvider
    {
        public AuthenticationProvider()
        {

        }



        /// <summary>
        /// Logs in user in the nanoCMS system
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //TODO: <dex> remember me option
        public bool SystemLogIn(int userId)
        {
            if (HttpContext.Current != null && !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                System.Web.Security.FormsAuthenticationTicket tkt;
                string cookiestr;
                System.Web.HttpCookie ck;

                tkt = new System.Web.Security.FormsAuthenticationTicket(2, userId.ToString(), DateTime.Now, DateTime.Now.AddDays(10), true, String.Empty);
                //tkt = new System.Web.Security.FormsAuthenticationTicket(userId.ToString(), true, 60);
                cookiestr = System.Web.Security.FormsAuthentication.Encrypt(tkt);
                ck = new System.Web.HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, cookiestr);
                if (true) ck.Expires = tkt.Expiration;
                ck.Path = System.Web.Security.FormsAuthentication.FormsCookiePath;
                HttpContext.Current.Response.Cookies.Add(ck);
                return true;
            }
            return false;
        }

        public void SystemLogOut()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
        }

    }
}
