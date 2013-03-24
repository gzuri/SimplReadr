using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimplReaderBLL.BLL.Concrete;
using SimplReaderBLL.BLL.Membership;
using SimplReaderBLL.Enumerators;

namespace SimplReaderMVC.Models.Account
{
    public class AccountService
    {
        private readonly AccountProvider accountProvider;
        private readonly AuthenticationProvider authenticationProvider;
        public AccountService(AccountProvider accountProvider, AuthenticationProvider authenticationProvider)
        {
            this.accountProvider = accountProvider;
            this.authenticationProvider = authenticationProvider;
        }
        /// <summary>
        /// User login on the system with email and password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AccountStatusEnum LogInUsingCredentials(LogOnVM model)
        {
            User user = accountProvider.GetUserByCredentials(model.Email, model.Password);
            if (user == null)
            {
                return AccountStatusEnum.UsernameAndPasswordMissmatch;
            }
            else if (!user.IsActivated)
            {
                return AccountStatusEnum.NotActivated;
            }
            else if (user.IsBlocked)
            {
                return AccountStatusEnum.UserBlocked;
            }
            return authenticationProvider.SystemLogIn(user.UserID) ? AccountStatusEnum.Successfull : AccountStatusEnum.UserAlreadyLoggedIn;
        }

        /// <summary>
        /// Logs user out of the system
        /// </summary>
        public void LogOut()
        {
            authenticationProvider.SystemLogOut();
        }
    }
}