using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimplReaderBLL.Enumerators;
using SimplReaderMVC.Models.Account;
using SimplReaderMVC.Resources;

namespace SimplReaderMVC.Controllers {
	public class AccountController : BaseController {
	    private readonly AccountService accountService;

	    public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }

		public ActionResult LogOn()
		{
		    
			return PartialView(new LogOnVM());
		}

		[HttpPost]
		public ActionResult LogOn(LogOnVM model) {
			if (ModelState.IsValid)
			{
			    var status = accountService.LogInUsingCredentials(model);
                switch (status)
                {
                    case AccountStatusEnum.Successfull:
                        AddNotification(Notifications.SuccessfullyLoggedIn, UserMessagesTypesEnum.Success);
                        return Redirect(Url.RouteUrl("DefaultNanoCms"));
                        break;
                    case AccountStatusEnum.UsernameAndPasswordMissmatch:
                        ModelState.AddModelError("", Translations.WrongEmailOrPassword);
                        break;
                    case AccountStatusEnum.NotActivated:
                        ModelState.AddModelError("", Translations.AccountNotActivated);
                        break;
                    case AccountStatusEnum.UserAlreadyLoggedIn:
                        ModelState.AddModelError("", Notifications.UserAlreadyLogedIn);
                        break;
                    default:
                        break;
                }
                return new JsonResult
                {
                    Data = new { status = ReturnStatusEnum.Success },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

			}
			return new JsonResult
			{
			    Data = new {status = ReturnStatusEnum.GenericError},
			    JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}

	}
}
