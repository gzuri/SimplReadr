using System.Web.Mvc;
using DotNetOpenAuth.OpenId.RelyingParty;
using SimplReaderBLL.Enumerators;
using SimplReaderMVC.Models.Account;
using SimplReaderMVC.Resources;

namespace SimplReaderMVC.Controllers {
	public class AccountController : BaseController {
	    private readonly AccountService accountService;
	    private OpenIdRelyingParty openID;

	    public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
            this.openID = new OpenIdRelyingParty();
        }

	    public ActionResult LogOn()
		{
            if (SimplReaderBLL.CurrentUser.IsAuthenticated)
                return RedirectToRoute("DefaultReader");
			return View(new LogOnVM());
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
                        return Redirect(Url.RouteUrl("DefaultReader"));
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

        public ActionResult LogOut()
        {
            accountService.LogOut();
            //AddNotification(Resources.Notification.SuccessfullyLoggedOut, UserMessagesTypesEnum.Information);
            return RedirectToRoute("LogOn");
        }


        public ActionResult RegisterWithOpenID()
        {
            var response = openID.GetResponse();
            if (response == null)
            {

            }
            return Content("ok");
        }

	    public ActionResult Register()
        {
            return View();
        }
	}
}
